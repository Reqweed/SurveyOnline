using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.DAL.Repositories.Contracts;

public interface ITopicRepository
{
    IQueryable<Topic> GetAllTopics(bool trackChanges);
    Task<Topic?> GetTopicByIdAsync(Guid idTopic, bool trackChanges);
    Task CreateTopicAsync(Topic topic);
    void UpdateTopic(Topic topic);
    void DeleteTopic(Topic topic);
}