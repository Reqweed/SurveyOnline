using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SurveyOnline.BLL.Entities.DTOs.Question;
using SurveyOnline.BLL.Entities.DTOs.Survey;
using SurveyOnline.BLL.Helpers.Contracts;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.BLL.Services.Implementations;

public class SurveyService(IRepositoryManager repositoryManager, ICloudinaryService cloudinaryService, IMapper mapper) : ISurveyService
{
    public async Task AddSurveyAsync(Guid idCreator, SurveyForCreatedDto surveyDto, List<QuestionForCreatedDto> questionsDto, List<Guid> idTags)
    {
        if (surveyDto is null)
            throw new Exception();
        if (questionsDto is null || questionsDto.Count == 0)
            throw new Exception();
        if (idTags is null)
            throw new Exception();

        var survey = mapper.Map<SurveyForCreatedDto, Survey>(surveyDto);
        var questions = mapper.Map<List<QuestionForCreatedDto>, List<Question>>(questionsDto);
        var tags = await repositoryManager.Tag.GetAllTags(true).Where(tag => idTags.Contains(tag.Id)).ToListAsync();

        survey.CreatorId = idCreator;
        survey.Questions = questions;
        foreach (var tag in tags)
        {
            survey.Tags.Add(tag);
        }
        survey.CreatedDate = DateTime.UtcNow;
        survey.UrlImage = await cloudinaryService.LoadImageAsync(surveyDto.Image);

        var topic = await repositoryManager.Topic.GetAllTopics(false).Where(t => t.Name == surveyDto.TopicName).FirstOrDefaultAsync();
        if (topic is null)
        {
            topic = new Topic { Name = surveyDto.TopicName };
            await repositoryManager.Topic.CreateTopicAsync(topic);
        }
        
        survey.TopicId = topic.Id;

        await repositoryManager.Survey.CreateSurveyAsync(survey);
        await repositoryManager.SaveAsync();
    }
}