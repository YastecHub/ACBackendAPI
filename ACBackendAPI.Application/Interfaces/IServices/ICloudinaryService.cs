using Microsoft.AspNetCore.Http;

namespace ACBackendAPI.Application.Interfaces.IServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
