namespace SurveyOnline.DAL.Entities.Models;

public class Survey
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public string? UrlImage { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CompletedCount { get; set; } // TODO

    public Guid TopicId { get; set; }
    public virtual Topic Topic { get; set; }

    public Guid CreatorId { get; set; }
    public virtual User Creator { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    public virtual ICollection<CompletedSurvey> CompletedSurveys { get; set; } = new List<CompletedSurvey>();
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public virtual ICollection<User> AccessibleUsers { get; set; } = new List<User>();
}