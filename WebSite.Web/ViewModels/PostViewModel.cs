using System.ComponentModel.DataAnnotations;

namespace WebSite.Web.ViewModels
{
    public class PostViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [DataType(DataType.Url)]
        public string Link { get; set; }

        public byte[] Cover { get; set; }
    }
}
