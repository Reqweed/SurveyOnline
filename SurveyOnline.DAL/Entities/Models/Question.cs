using SurveyOnline.DAL.Entities.Enums;

namespace SurveyOnline.DAL.Entities.Models;

public class Question
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public QuestionType Type { get; set; }
    public bool IsVisible { get; set; }

    public Guid SurveyId { get; set; }
    public virtual Survey Survey { get; set; }
}