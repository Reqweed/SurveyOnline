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
    public Topic Topic { get; set; }

    public Guid CreatorId { get; set; }
    public User Creator { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<CompletedSurvey> CompletedSurveys { get; set; } = new List<CompletedSurvey>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<User> AccessibleUsers { get; set; } = new List<User>();
}