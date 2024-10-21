namespace SurveyOnline.DAL.Entities.Models;

public class CompletedSurvey
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid SurveyId { get; set; }
    public Survey Survey { get; set; }

    public ICollection<Answer> Answers { get; set; }
}