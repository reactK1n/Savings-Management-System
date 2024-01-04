using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SavingsManagementSystem.Common.Model;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.Files.Interfaces;

namespace SavingsManagementSystem.Service.Files.Implementations
{
	public class ImageService : IImageService
	{
		private readonly IConfiguration _config;
		private readonly Cloudinary _cloudinary;
		private readonly ImageSettings _imageSettings;

		public ImageService(IConfiguration config, IUnitOfWork unit)
		{
			_config = config;
			_imageSettings = config.GetSection("CloudinarySettings").Get<ImageSettings>();
			_cloudinary = new Cloudinary(new Account(
				_imageSettings.CloudName,
				_imageSettings.ApiKey,
				_imageSettings.ApiSecret));
		}
		public async Task<string> UploadImageAsync(IFormFile image)
		{
			var imageExtension = _config.GetSection("PhotoSettings:Formats").Get<List<string>>();
			var fileFormat = false;
			foreach (var item in imageExtension)
			{
				if (image.FileName.ToLower().EndsWith(item))
				{
					fileFormat = true;
					break;
				}
			}

			if (!fileFormat)
			{
				throw new NotSupportedException("file format not supported");

			}

			var uploadResult = new ImageUploadResult();
			using (var imageStream = image.OpenReadStream())
			{
				string fileName = $"{Guid.NewGuid()}{image.FileName}";
				uploadResult = await _cloudinary.UploadAsync(new ImageUploadParams
				{
					File = new FileDescription(fileName, imageStream),
					Transformation = new Transformation().Radius("max").Chain().Crop("scale").Width("200").Height("200")
				});
			}
			var picsUri = uploadResult.Uri.ToString();
			return picsUri;
		}

	}
}
