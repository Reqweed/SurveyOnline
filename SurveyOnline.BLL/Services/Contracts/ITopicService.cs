using SurveyOnline.BLL.Entities.DTOs.Topic;

namespace SurveyOnline.BLL.Services.Contracts;

public interface ITopicService
{
    Task<IEnumerable<TopicDto>> GetTopicsAsync();
}