using Microsoft.AspNetCore.Http;

namespace SurveyOnline.BLL.Helpers.Contracts;

public interface ICloudinaryService
{
    Task<string> LoadImageAsync(IFormFile image);
}