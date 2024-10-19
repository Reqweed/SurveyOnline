using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyOnline.BLL.Entities.DTOs.Question;
using SurveyOnline.BLL.Entities.DTOs.Survey;
using SurveyOnline.BLL.Helpers.Contracts;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.BLL.Services.Implementations;

public class SurveyService(SignInManager<User> signInManager, IRepositoryManager repositoryManager,
    ICloudinaryService cloudinaryService, IMapper mapper) : ISurveyService
{
    public async Task CreateSurveyAsync(Guid idCreator, SurveyForCreatedDto surveyDto,
        List<QuestionForCreatedDto> questionsDto, List<Guid> tagIds, List<Guid> userIds)
    {
        ValidateInput(surveyDto, questionsDto, tagIds);

        var survey = MapSurvey(surveyDto, idCreator, questionsDto);

        await AssignTagsToSurvey(survey, tagIds);
        await AssignTopicToSurvey(survey, surveyDto.TopicName);
        await AssignImageToSurvey(survey, surveyDto.Image);
        await AssignUsersToSurvey(survey, userIds);

        await repositoryManager.Survey.CreateSurveyAsync(survey);
        await repositoryManager.SaveAsync();
    }

    public async Task<IEnumerable<SurveyDto>> GetTopCompletedSurveysAsync(Guid idUser, int countSurvey)
    {
        var surveysTop = await GetAccessibleSurveys(idUser)
            .OrderByDescending(survey => survey.CompletedCount)
            .Take(countSurvey)
            .ToListAsync();

        return mapper.Map<IEnumerable<Survey>, IEnumerable<SurveyDto>>(surveysTop);
    }

    public async Task<IEnumerable<SurveyDto>> GetPagedAccessibleSurveysAsync(Guid idUser, int currentPage, int pageSize)
    {
        var surveys = await GetAccessibleSurveys(idUser)
            .OrderBy(survey => survey.CreatedDate)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return mapper.Map<IEnumerable<Survey>, IEnumerable<SurveyDto>>(surveys);
    }

    private void ValidateInput(SurveyForCreatedDto surveyDto, List<QuestionForCreatedDto> questionsDto,
        List<Guid> tagIds)
    {
        if (surveyDto == null)
            throw new Exception("Survey is null");
        if (questionsDto == null || questionsDto.Count == 0)
            throw new Exception("Questions are missing");
        if (tagIds == null)
            throw new Exception("Tags are missing");
    }

    private Survey MapSurvey(SurveyForCreatedDto surveyDto, Guid idCreator, List<QuestionForCreatedDto> questionsDto)
    {
        var survey = mapper.Map<SurveyForCreatedDto, Survey>(surveyDto);

        survey.CreatorId = idCreator;
        survey.Questions = mapper.Map<List<QuestionForCreatedDto>, List<Question>>(questionsDto);
        survey.CreatedDate = DateTime.UtcNow;

        return survey;
    }

    private async Task AssignTagsToSurvey(Survey survey, List<Guid> tagIds)
    {
        var tags = await repositoryManager.Tag.GetAllTags(true)
            .Where(tag => tagIds.Contains(tag.Id))
            .ToListAsync();

        foreach (var tag in tags)
        {
            survey.Tags.Add(tag);
        }
    }

    private async Task AssignTopicToSurvey(Survey survey, string topicName)
    {
        var topic = await repositoryManager.Topic.GetAllTopics(false)
            .FirstOrDefaultAsync(t => t.Name == topicName);

        if (topic == null)
        {
            topic = new Topic { Name = topicName };
            await repositoryManager.Topic.CreateTopicAsync(topic);
        }

        survey.TopicId = topic.Id;
    }

    private async Task AssignImageToSurvey(Survey survey, IFormFile? image)
    {
        if (image is not null)
        {
            survey.UrlImage = await cloudinaryService.LoadImageAsync(image);
        }
    }

    private async Task AssignUsersToSurvey(Survey survey, List<Guid>? userIds)
    {
        if (userIds is not null && userIds.Count != 0)
        {
            var users = await signInManager.UserManager.Users
                .Where(user => userIds.Contains(user.Id))
                .ToListAsync();

            survey.AccessibleUsers = users;
        }
    }

    private IQueryable<Survey> GetAccessibleSurveys(Guid idUser)
    {
        return repositoryManager.Survey.GetAllSurveys(false)
            .Where(survey => survey.IsPublic || survey.AccessibleUsers.Any(user => user.Id == idUser)
                                             || survey.CreatorId == idUser);
    }
}