using System;
using System.IO;
using Core.Infrastructure.Abstract;
using Microsoft.AspNetCore.Hosting;

namespace BlogProje.Infrastructure
{
    public class LocalImageStorage : IImageStorage
    {
        private readonly IWebHostEnvironment _env;
        public LocalImageStorage(IWebHostEnvironment env) => _env = env;

        public void DeleteBlogImages(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            // Güvenlik: sadece dosya adı kalsın (path gelirse soy)
            var safeName = Path.GetFileName(fileName);

            var bigPath = Path.Combine(_env.WebRootPath, "images", "BlogImages", "big", safeName);
            var smallPath = Path.Combine(_env.WebRootPath, "images", "BlogImages", "small", safeName);

            TryDelete(bigPath);
            TryDelete(smallPath);
        }

        private static void TryDelete(string fullPath)
        {
            try
            {
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }
            catch
            {
                // loglamak istersen burada ILogger kullanabilirsin
                // şimdilik sessiz geçiyoruz (best-effort)
            }
        }
    }
}
