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
    public async Task CreateSurveyAsync(SurveyForCreatedDto surveyDto,
        List<QuestionForCreatedDto> questionsDto, List<Guid> tagIds, List<Guid> userIds)
    {
        ValidateInput(surveyDto, questionsDto, tagIds);

        var survey = MapSurvey(surveyDto, questionsDto);

        await AssignCreatorToSurvey(survey);
        await AssignTagsToSurvey(survey, tagIds);
        await AssignTopicToSurvey(survey, surveyDto.TopicName);
        await AssignImageToSurvey(survey, surveyDto.Image);
        await AssignUsersToSurvey(survey, userIds);

        await repositoryManager.Survey.CreateSurveyAsync(survey);
        await repositoryManager.SaveAsync();
    }

    public async Task<IEnumerable<SurveyDto>> GetTopCompletedSurveysAsync(int countSurvey)
    {
        var surveysTop = await (await GetAccessibleSurveys())
            .OrderByDescending(survey => survey.CompletedCount)
            .Take(countSurvey)
            .ToListAsync();

        return mapper.Map<IEnumerable<Survey>, IEnumerable<SurveyDto>>(surveysTop);
    }

    public async Task<IEnumerable<SurveyDto>> GetPagedAccessibleSurveysAsync(int currentPage, int pageSize)
    {
        var surveys = await (await GetAccessibleSurveys())
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

    private Survey MapSurvey(SurveyForCreatedDto surveyDto, List<QuestionForCreatedDto> questionsDto)
    {
        var survey = mapper.Map<SurveyForCreatedDto, Survey>(surveyDto);

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

    private async Task AssignCreatorToSurvey(Survey survey)
    {
        var creator = await signInManager.UserManager.GetUserAsync(signInManager.Context.User);
        if (creator is null)
            throw new Exception();

        survey.Creator = creator;
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

    private async Task<IQueryable<Survey>> GetAccessibleSurveys()
    {
        var user = await signInManager.UserManager.GetUserAsync(signInManager.Context.User);
        var surveys = repositoryManager.Survey.GetAllSurveys(false);

        if (user is null)
        {
            return surveys.Where(survey => survey.IsPublic);
        }

        var userId = user.Id;

        return surveys.Where(survey =>
            survey.IsPublic || survey.CreatorId == userId || survey.AccessibleUsers.Any(u => u.Id == userId));
    }
}