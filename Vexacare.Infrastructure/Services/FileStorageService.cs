using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Vexacare.Application.Interfaces;

namespace Vexacare.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileStorageService(
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string containerName)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(_env.WebRootPath, "images", containerName);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var path = Path.Combine(folder, fileName);
            //using (var memoryStream = new MemoryStream())
            //{
            //    await file.CopyToAsync(memoryStream);
            //    var content = memoryStream.ToArray();
            //    await File.WriteAllBytesAsync(path, content);
            //}
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/images/{containerName}/{fileName}";

            //var currentUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            //return Path.Combine(currentUrl, containerName, fileName).Replace("\\", "/");

        }

        public Task DeleteFileAsync(string filePath, string containerName)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return Task.CompletedTask;
            }

            var fileName = Path.GetFileName(filePath);
            var fileDirectory = Path.Combine(_env.WebRootPath, containerName, fileName);

            if (File.Exists(fileDirectory))
            {
                File.Delete(fileDirectory);
            }

            return Task.CompletedTask;
        }

        public async Task<string> UpdateFileAsync(IFormFile newFile, string oldFilePath, string containerName)
        {
            await DeleteFileAsync(oldFilePath, containerName);
            return await SaveFileAsync(newFile, containerName);
        }
    }
}
