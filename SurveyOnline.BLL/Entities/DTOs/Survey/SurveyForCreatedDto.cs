using Microsoft.AspNetCore.Http;

namespace SurveyOnline.BLL.Entities.DTOs.Survey;

public class SurveyForCreatedDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string TopicName { get; set; }
    public bool IsPublic { get; set; }
    public IFormFile Image { get; set; }
}