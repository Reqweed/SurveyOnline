namespace SurveyOnline.DAL.Entities.Models;

public class CompletedSurvey
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; }

    public Guid SurveyId { get; set; }
    public virtual Survey Survey { get; set; }

    public virtual ICollection<Answer> Answers { get; set; }
}