using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SurveyOnline.BLL.Entities.DTOs.Survey;

public class SurveyForCreatedDto
{
    [Required]
    [MaxLength(20)]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    [MaxLength(30)]
    public string TopicName { get; set; }
    public bool IsPublic { get; set; }
    public IFormFile? Image { get; set; }
}