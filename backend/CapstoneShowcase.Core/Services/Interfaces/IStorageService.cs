using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CapstoneShowcase.Core.Services.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
        Task DeleteFileAsync(string fileUrl);
    }
}