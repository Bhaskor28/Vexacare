using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Vexacare.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string containerName);
        Task DeleteFileAsync(string filePath, string containerName);
        Task<string> UpdateFileAsync(IFormFile newFile, string oldFilePath, string containerName);
    }
}
