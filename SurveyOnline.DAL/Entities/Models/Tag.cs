namespace SurveyOnline.DAL.Entities.Models;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Survey> Surveys { get; set; }
}