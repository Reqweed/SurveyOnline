namespace SurveyOnline.DAL.Entities.Models;

public class Answer
{
    public Guid Id { get; set; }
    public string AnswerValue { get; set; }

    public Guid CompletedSurveyId { get; set; }
    public CompletedSurvey CompletedSurvey { get; set; }

    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
}