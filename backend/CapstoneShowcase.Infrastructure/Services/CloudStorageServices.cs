using CapstoneShowcase.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Services
{
    public class CloudStorageService : IStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly string _storageBasePath;
        
        public CloudStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            
            // In a real implementation, this would use a cloud storage provider
            // For this example, we'll use local storage
            _storageBasePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            
            // Ensure the directory exists
            if (!Directory.Exists(_storageBasePath))
            {
                Directory.CreateDirectory(_storageBasePath);
            }
        }
        
        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;
                
            var folderPath = Path.Combine(_storageBasePath, folderName);
            
            // Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            // Generate a unique filename
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);
            
            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            // Generate the URL for the file
            // In a real cloud implementation, this would be the URL from the cloud provider
            return $"/uploads/{folderName}/{fileName}";
        }
        
        public Task DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return Task.CompletedTask;
                
            // Extract the file path from the URL
            var relativePath = fileUrl.TrimStart('/');
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            
            return Task.CompletedTask;
        }
    }
}