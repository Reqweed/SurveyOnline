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
    ICloudinaryService cloudinaryService, ISurveySearchService surveySearchService, IMapper mapper) : ISurveyService
{
    public async Task CreateSurveyAsync(SurveyForCreatedDto surveyDto,
        List<QuestionForCreatedDto> questionsDto, List<string> tagNames, List<Guid> userIds)
    {
        ValidateInput(surveyDto, questionsDto, tagNames);

        var survey = await ConstructSurveyAsync(surveyDto, questionsDto, tagNames, userIds);

        await repositoryManager.Survey.CreateSurveyAsync(survey);

        await AddSurveyForSearchAsync(survey);

        await repositoryManager.SaveAsync();
    }

    public async Task<IEnumerable<SurveyDto>> GetTopCompletedSurveysAsync(int countSurvey)
    {
        var surveysTop = await (await GetAccessibleSurveysAsync())
            .OrderByDescending(survey => survey.CompletedCount)
            .Take(countSurvey)
            .Include(survey => survey.Creator)
            .ToListAsync();

        return mapper.Map<IEnumerable<Survey>, IEnumerable<SurveyDto>>(surveysTop);
    }

    public async Task<IEnumerable<SurveyDto>> GetPagedAccessibleSurveysAsync(int currentPage, int pageSize)
    {
        var surveys = await (await GetAccessibleSurveysAsync())
            .OrderByDescending(survey => survey.CreatedDate)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .Include(survey => survey.Creator)
            .ToListAsync();

        return mapper.Map<IEnumerable<Survey>, IEnumerable<SurveyDto>>(surveys);
    }

    public async Task<SurveyForCompletedDto> GetSurveyAsync(Guid idSurvey)
    {
        var survey = await (await GetAccessibleSurveysAsync())
            .Where(survey => survey.Id == idSurvey)
            .Include(survey => survey.Creator)
            .Include(survey => survey.Questions)
            .Include(survey => survey.Topic)
            .Include(survey => survey.Tags)
            .FirstOrDefaultAsync();
        if (survey is null)
            throw new KeyNotFoundException($"Survey with ID '{idSurvey}' not found.");

        var surveyDto = mapper.Map<Survey, SurveyForCompletedDto>(survey);

        return surveyDto;
    }

    public async Task<IEnumerable<SurveyDto>> GetSurveysByTagAsync(string tag)
    {
        var surveysIndexDto = await surveySearchService.SearchSurveyByTagAsync(tag);

        var surveysDto = mapper.Map<IEnumerable<SurveyForIndexDto>, IEnumerable<SurveyDto>>(surveysIndexDto);

        return surveysDto;
    }

    public async Task<IEnumerable<SurveyDto>> GetSurveysByTermAsync(string term)
    {
        var surveysIndexDto = await surveySearchService.SearchSurveysAsync(term);

        var surveysDto = mapper.Map<IEnumerable<SurveyForIndexDto>, IEnumerable<SurveyDto>>(surveysIndexDto);

        return surveysDto;
    }

    private void ValidateInput(SurveyForCreatedDto surveyDto, List<QuestionForCreatedDto> questionsDto,
        List<string> tagNames)
    {
        if (surveyDto == null)
            throw new ArgumentNullException(nameof(surveyDto), "Survey cannot be null.");
        if (questionsDto == null || questionsDto.Count == 0)
            throw new ArgumentException("At least one question is required.", nameof(questionsDto));
        if (tagNames == null || tagNames.Count == 0)
            throw new ArgumentException("At least one tag is required.", nameof(tagNames));
    }

    private async Task<Survey> ConstructSurveyAsync(SurveyForCreatedDto surveyDto,
        List<QuestionForCreatedDto> questionsDto, List<string> tagNames, List<Guid> userIds)
    {
        var survey = mapper.Map<SurveyForCreatedDto, Survey>(surveyDto);

        await AssignCreatorToSurveyAsync(survey);
        await AssignTagsToSurveyAsync(survey, tagNames);
        await AssignTopicToSurveyAsync(survey, surveyDto.TopicName);
        await AssignImageToSurveyAsync(survey, surveyDto.Image);
        await AssignUsersToSurveyAsync(survey, userIds);
        survey.CreatedDate = DateTime.UtcNow;
        survey.Questions = mapper.Map<List<QuestionForCreatedDto>, List<Question>>(questionsDto);

        return survey;
    }

    private async Task AssignTagsToSurveyAsync(Survey survey, List<string> tagNames)
    {
        var existingTags = await repositoryManager.Tag.GetAllTags(true)
            .Where(tag => tagNames.Contains(tag.Name))
            .ToListAsync();

        var newTagNames = tagNames.Except(existingTags.Select(t => t.Name)).ToList();

        var newTags = newTagNames.Select(tagName => new Tag { Name = tagName }).ToList();
        existingTags.AddRange(newTags);

        survey.Tags = existingTags;
    }

    private async Task AssignTopicToSurveyAsync(Survey survey, string topicName)
    {
        var topic = await repositoryManager.Topic.GetAllTopics(true)
            .FirstOrDefaultAsync(t => t.Name == topicName);
        if (topic == null)
            throw new ArgumentException("Topic not found.", nameof(topicName));

        survey.Topic = topic;
    }

    private async Task AssignImageToSurveyAsync(Survey survey, IFormFile? image)
    {
        if (image is not null)
        {
            survey.UrlImage = await cloudinaryService.LoadImageAsync(image);
        }
    }

    private async Task AssignCreatorToSurveyAsync(Survey survey)
    {
        var creator = await signInManager.UserManager.GetUserAsync(signInManager.Context.User);
        if (creator is null)
            throw new InvalidOperationException("User not found or not authenticated.");

        survey.Creator = creator;
    }

    private async Task AssignUsersToSurveyAsync(Survey survey, List<Guid>? userIds)
    {
        if (userIds is not null && userIds.Count != 0)
        {
            var users = await signInManager.UserManager.Users
                .Where(user => userIds.Contains(user.Id))
                .ToListAsync();

            survey.AccessibleUsers = users;
        }
    }

    private async Task AddSurveyForSearchAsync(Survey survey)
    {
        var surveyIndexDto = mapper.Map<Survey, SurveyForIndexDto>(survey);
        await surveySearchService.AddIndexAsync(surveyIndexDto);
    }

    private async Task<IQueryable<Survey>> GetAccessibleSurveysAsync()
    {
        var user = await signInManager.UserManager.GetUserAsync(signInManager.Context.User);
        var surveys = repositoryManager.Survey.GetAllSurveys(false);

        if (user is null)
        {
            return surveys.Where(survey => survey.IsPublic);
        }

        return surveys.Where(survey =>
            survey.IsPublic || survey.CreatorId == user.Id || survey.AccessibleUsers.Any(u => u.Id == user.Id));
    }
}