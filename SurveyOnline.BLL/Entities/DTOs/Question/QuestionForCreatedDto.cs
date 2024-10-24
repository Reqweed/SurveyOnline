using System.ComponentModel.DataAnnotations;
using SurveyOnline.DAL.Entities.Enums;

namespace SurveyOnline.BLL.Entities.DTOs.Question;

public class QuestionForCreatedDto
{
    [Required]
    [MaxLength(20)]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    public QuestionType Type { get; set; }
    public bool IsVisible { get; set; }
}