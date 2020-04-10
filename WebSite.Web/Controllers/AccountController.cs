using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSite.Core.Entities;
using WebSite.Core.Interfaces;
using WebSite.Data.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebSite.DataAcces.Identity;
using WebSite.Web.ViewModels;
using WebSite.Extensions;

namespace WebSite.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAsyncRepository<AuthorEntity> AuthorEntityRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(IAsyncRepository<AuthorEntity> AuthorEntityRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.AuthorEntityRepository = AuthorEntityRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Settings(string id)
        {
            if (id != this.User.Identity.GetAuthorEntityName())
            {
                return this.RedirectTo<HomeController>(x => x.NoAccess());
            }

            var user = this.userManager.Users.FirstOrDefault(u => u.UserName == id);
            ViewBag.CurrentUserPicture = user.AuthorEntity.Image;

            return View();
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(string id, ProfileViewModel profileViewModel)
        {
            var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.UserName == id);

            if (ModelState.IsValid)
            {
                if (!(profileViewModel.Profile == null || profileViewModel.Profile.Length <= 0))
                {
                    user.AuthorEntity.Image = profileViewModel.Profile.ToByteArray();
                }

                await this.userManager.UpdateAsync(user);
                return this.RedirectToAsync<AccountController>(x => x.Profile(id));
            }

            ViewBag.CurrentUserPicture = user.AuthorEntity.Image;
            return View(profileViewModel);
        }


        public async Task<IActionResult> Profile(string id)
        {
            var user = await this.userManager.FindByNameAsync(id);
            return View(user.AuthorEntity);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if (registerViewModel.Password.Equals(registerViewModel.ConfirmPassword))
                {
                    var AuthorEntity = await this.AuthorEntityRepository.Add(new AuthorEntity
                    {
                        Id = Guid.NewGuid(),
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        Name = registerViewModel.UserName,
                        Image = System.IO.File.ReadAllBytes(".//wwwroot//images//developer-solid-2-17-512.png")
                    });

                    var applicationUser = new ApplicationUser
                    {
                        UserName = registerViewModel.UserName,
                        Email = registerViewModel.Email,
                        AuthorEntity = AuthorEntity
                    };

                    var result = await userManager.CreateAsync(applicationUser, registerViewModel.Password);

                    if (result.Succeeded)
                    {
                        await this.signInManager.PasswordSignInAsync(
                            registerViewModel.UserName,
                            registerViewModel.Password,
                            registerViewModel.RememberMe,
                            false);

                        return this.RedirectToAsync<HomeController>(x => x.Index());
                    }
                    else
                    {
                        ModelState.AddModelError("", "Something dosen´t work.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Passwords dosen´t are the same.");
                }
            }

            return View(registerViewModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string userName = loginViewModel.UserName;

                if (userName.IndexOf("@") > -1)
                {
                    var user = await this.userManager.FindByEmailAsync(userName);

                    if (user == null)
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(loginViewModel);
                    }
                    else
                    {
                        userName = user.UserName;
                    }
                }

                var result = await signInManager.PasswordSignInAsync(userName, loginViewModel.Password, loginViewModel.RememberMe, false);

                if (result.Succeeded)
                {
                    return this.RedirectToAsync<HomeController>(x => x.Index());
                }
                else
                {
                    ModelState.AddModelError("", "Password or Username is not right.");
                }
            }

            return View(loginViewModel);
        }


        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return this.RedirectToAsync<HomeController>(x => x.Index());
        }
    }
}