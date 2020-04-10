using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSite.Core.Entities;
using WebSite.Core.Interfaces;
using WebSite.DataAcces.Identity;
using System;
using System.Threading.Tasks;
using WebSite.Web.ViewModels;
using WebSite.Extensions;
using WebSite.Data.Extensions;

namespace WebSite.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IAsyncRepository<BlogEntity> blogRepository;

        public BlogController(IAsyncRepository<BlogEntity> blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        public async Task<IActionResult> View(Guid id)
        {
            var blog = await this.blogRepository.GetById(id);
            return View(blog);
        }

        [Authorize(Roles = ApplicationRoles.Administrator)]
        public IActionResult New()
        {
            return View();
        }

        [Authorize(Roles = ApplicationRoles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(BlogViewModel blog, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                if (formFile != null)
                {
                    blog.Cover = formFile.ToByteArray();
                }
                else
                {
                    blog.Cover = System.IO.File.ReadAllBytes(".//wwwroot//images//blog-logo-7E0C67F0D1-seeklogo.com.png");
                }

                var newBlog = await this.blogRepository.Add(new BlogEntity
                {
                    Id = Guid.NewGuid(),
                    Name = blog.Name,
                    Description = blog.Description,
                    Cover = blog.Cover,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                });

                return this.RedirectToAsync<BlogController>(x => x.View(newBlog.Id));
            }
            return View(blog);
        }


        [Authorize(Roles = ApplicationRoles.Administrator)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blog = await this.blogRepository.GetById(id);

            return View(new BlogViewModel
            {
                Name = blog.Name,
                Cover = blog.Cover,
                Description = blog.Description
            });
        }

        [Authorize(Roles = ApplicationRoles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BlogViewModel blog, IFormFile formFile)
        {
            var targetBlog = await this.blogRepository.GetById(id);

            if (ModelState.IsValid)
            {
                targetBlog.Name = blog.Name;
                targetBlog.Description = blog.Description;

                if (!(formFile == null || formFile.Length <= 0))
                {
                    targetBlog.Cover = formFile.ToByteArray();
                }

                await this.blogRepository.Update(targetBlog);
                return this.RedirectToAsync<BlogController>(x => x.View(targetBlog.Id));
            }

            blog.Cover = targetBlog.Cover;

            return View(blog);
        }


        [Authorize(Roles = ApplicationRoles.Administrator)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.blogRepository.Remove(id);
            return this.RedirectToAsync<HomeController>(x => x.Index());
        }
    }
}