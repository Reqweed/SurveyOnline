using SurveyOnline.DAL.Entities.Enums;

namespace SurveyOnline.BLL.Entities.DTOs.Question;

public class QuestionForCreatedDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public QuestionType Type { get; set; }
    public bool IsVisible { get; set; }
}