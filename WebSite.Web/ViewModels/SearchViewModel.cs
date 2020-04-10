using WebSite.Core.Entities;
using System.Collections.Generic;

namespace WebSite.Web.ViewModels
{
    public class SearchViewModel
    {
        public IReadOnlyList<PostEntity> Posts { get; set; }

        public IReadOnlyList<BlogEntity> Blogs { get; set; }

        public IReadOnlyList<AuthorEntity> Users { get; set; }
    }
}
