using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SurveyOnline.BLL.Entities.DTOs.Topic;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.BLL.Services.Implementations;

public class TopicService(IRepositoryManager repositoryManager, IMapper mapper) : ITopicService
{
    public async Task<IEnumerable<TopicDto>> GetAllTopicsAsync()
    {
        var topics = await repositoryManager.Topic.GetAllTopics(false).ToListAsync();

        var topicsDto = mapper.Map<IEnumerable<Topic>,IEnumerable<TopicDto>>(topics);

        return topicsDto;
    }
}