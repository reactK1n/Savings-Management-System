using Microsoft.AspNetCore.Http;

namespace SavingsManagementSystem.Service.Files.Interfaces
{
    public interface IImageService
    {
		Task<string> UploadImageAsync(IFormFile image);

	}
}