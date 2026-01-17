using HortaGestao.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HortaGestao.Application.Services;

public class StorageService:IStorageService
{
    private readonly string _rootPath;
    private readonly string _virtualPath;
    
    public StorageService(IConfiguration configuration)
    {
        _rootPath = configuration["StorageConfig:Path"];
        _virtualPath = configuration["StorageConfig:VirtualPath"];
    }
    
    public async Task<string> SaveFileAsync(IFormFile file,  string containerName)
    {
        var targetPath = Path.Combine(_rootPath, containerName);
        
        if(!Directory.Exists(targetPath))
            Directory.CreateDirectory(targetPath);

        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = Guid.NewGuid() + fileExtension;
        var filePath = Path.Combine(targetPath, fileName);

        await using (var FileStream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(FileStream);
        
        return GetUrl(containerName, fileName);
    }

    public Task DeleteFileAsync(string fileName, string containerName)
    {
        try
        {
            var fileSplit = fileName.Split("/");
            var filePath = Path.Combine(_rootPath, containerName, fileSplit[4]);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (Exception e)
        {
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string containerName)
    {
        if (file == null || file.Length == 0) return null;

        var targetPath = Path.Combine(_rootPath, containerName);
        if(!Directory.Exists(targetPath))
            Directory.CreateDirectory(targetPath);

        
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(targetPath, fileName);
        
        await using (var FileStream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(FileStream);

        return GetUrl(containerName, fileName);

    }

    public async Task<byte[]> GetFileAsync(string fileName, string containerName)
    {
        var filePath = Path.Combine(_rootPath, containerName, fileName);
        
        if (!File.Exists(filePath))
        {
           throw new Exception("not found.");
        }
        
        return await File.ReadAllBytesAsync(filePath);
    }
    
    private string GetUrl(string containerName, string fileName)
    {
        var fullPath = $"{_virtualPath}/{containerName}/{fileName}";
        return fullPath;
    }
}