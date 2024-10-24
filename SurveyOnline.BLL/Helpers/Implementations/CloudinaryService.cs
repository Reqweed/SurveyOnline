using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using SurveyOnline.BLL.Helpers.Contracts;

namespace SurveyOnline.BLL.Helpers.Implementations;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService()
    {
        var account = new Account(
            Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
            Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
            Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> LoadImageAsync(IFormFile image)
    {
        if (image == null || image.Length == 0)
            throw new ArgumentException("Image is required.", nameof(image));

        await using var stream = image.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(image.FileName, stream),
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
            throw new InvalidOperationException(uploadResult.Error.Message);

        return uploadResult.SecureUrl.ToString();
    }
}