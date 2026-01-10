using Microsoft.AspNetCore.Http;

namespace HortaGestao.Application.Interfaces.Services;

public interface IStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string containerName);
    Task DeleteFileAsync(string fileName, string containerName);
    Task<string> UploadFileAsync(IFormFile file, string containerName);
    Task<byte[]> GetFileAsync(string fileName, string containerName);
}