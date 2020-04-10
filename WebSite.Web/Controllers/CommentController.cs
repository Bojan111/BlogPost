using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSite.Core.Entities;
using WebSite.Core.Interfaces;
using WebSite.DataAcces.Identity;
using System;
using System.Threading.Tasks;
using WebSite.Data.Extensions;

namespace WebSite.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly IAsyncRepository<ReplyEntity> commentRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentController(IAsyncRepository<ReplyEntity> commentRepository, UserManager<ApplicationUser> userManager)
        {
            this.commentRepository = commentRepository;
            this.userManager = userManager;
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> New(Guid id, string CommentText)
        {
            if (string.IsNullOrWhiteSpace(CommentText))
            {
                ModelState.AddModelError("", "Text field is required!");
                return this.RedirectToAsync<PostController>(x => x.Details(id));
            }

            var newComment = await this.commentRepository.Add(new ReplyEntity
            {
                Content = CommentText,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                AuthorId = User.Identity.GetAuthorId(),
                PostId = id
            });

            return this.RedirectToAsync<PostController>(x => x.Details(newComment.PostId.Value));
        }


        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = await this.commentRepository.GetById(id);

            if (User.Identity.GetAuthorId() != comment.AuthorId)
            {
                return this.RedirectTo<HomeController>(x => x.NoAccess());
            }

            await this.commentRepository.Remove(id);
            return this.RedirectToAsync<PostController>(x => x.Details(comment.PostId.Value));
        }


        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            var comment = await this.commentRepository.GetById(id);

            if (!(User.Identity.GetAuthorId() == comment.AuthorId))
            {
                return this.RedirectTo<HomeController>(x => x.NoAccess());
            }

            return View(comment);
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> Edit(Guid id, ReplyEntity comment)
        {
            if (ModelState.IsValid)
            {
                var targetComment = await this.commentRepository.GetById(id);
                targetComment.Content = comment.Content;

                await this.commentRepository.Update(targetComment);

                return this.RedirectToAsync<PostController>(x => x.Details(targetComment.PostId.Value));
            }

            return View(comment);
        }

    }
}