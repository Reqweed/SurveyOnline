using AutoMapper;
using SurveyOnline.BLL.Entities.DTOs.Topic;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.BLL.Services.Implementations;

public class TopicService(IRepositoryManager repositoryManager, IMapper mapper) : ITopicService
{
    public IEnumerable<TopicDto> GetAllTopics()
    {
        var topics = repositoryManager.Topic.GetAllTopics(false).ToList();

        var topicsDto = mapper.Map<IEnumerable<Topic>,IEnumerable<TopicDto>>(topics);

        return topicsDto;
    }
}