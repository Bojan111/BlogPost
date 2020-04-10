using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebSite.Extensions
{
    public static class FileExtensions
    {
        public static byte[] ToByteArray(this IFormFile formFile)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
