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

        var survey = MapSurvey(surveyDto, questionsDto);

        await AssignCreatorToSurvey(survey);
        await AssignTagsToSurvey(survey, tagNames);
        await AssignTopicToSurvey(survey, surveyDto.TopicName);
        await AssignImageToSurvey(survey, surveyDto.Image);
        await AssignUsersToSurvey(survey, userIds);
        
        await repositoryManager.Survey.CreateSurveyAsync(survey);
        
        await AddSurveyForSearch(survey);
        
        await repositoryManager.SaveAsync();
    }

    public async Task<IEnumerable<SurveyDto>> GetTopCompletedSurveysAsync(int countSurvey)
    {
        var surveysTop = await (await GetAccessibleSurveys())
            .OrderByDescending(survey => survey.CompletedCount)
            .Take(countSurvey)
            .Include(survey => survey.Creator)
            .ToListAsync();

        return mapper.Map<IEnumerable<Survey>, IEnumerable<SurveyDto>>(surveysTop);
    }

    public async Task<IEnumerable<SurveyDto>> GetPagedAccessibleSurveysAsync(int currentPage, int pageSize)
    {
        var surveys = await (await GetAccessibleSurveys())
            .OrderByDescending(survey => survey.CreatedDate)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .Include(survey => survey.Creator)
            .ToListAsync();

        return mapper.Map<IEnumerable<Survey>, IEnumerable<SurveyDto>>(surveys);
    }

    public async Task<SurveyForCompletedDto> GetSurveyAsync(Guid idSurvey)
    {
        var survey = await (await GetAccessibleSurveys())
            .Where(survey => survey.Id == idSurvey)
            .Include(survey => survey.Creator)
            .Include(survey => survey.Questions)
            .Include(survey => survey.Topic)
            .Include(survey => survey.Tags)
            .FirstOrDefaultAsync();
        if (survey is null)
            throw new Exception();

        var surveyDto = mapper.Map<Survey, SurveyForCompletedDto>(survey);

        return surveyDto;
    }

    public async Task<IEnumerable<SurveyDto>> GetSurveysByTag(string tag)
    {
        var surveysIndexDto = await surveySearchService.SearchSurveyByTagAsync(tag);

        var surveysDto = mapper.Map<IEnumerable<SurveyForIndexDto>, IEnumerable<SurveyDto>>(surveysIndexDto);

        return surveysDto;
    }    
    
    public async Task<IEnumerable<SurveyDto>> GetSurveysByTerm(string term)
    {
        var surveysIndexDto = await surveySearchService.SearchSurveysAsync(term);

        var surveysDto = mapper.Map<IEnumerable<SurveyForIndexDto>, IEnumerable<SurveyDto>>(surveysIndexDto);

        return surveysDto;
    }

    private void ValidateInput(SurveyForCreatedDto surveyDto, List<QuestionForCreatedDto> questionsDto,
        List<string> tagNames)
    {
        if (surveyDto == null)
            throw new Exception("Survey is null");
        if (questionsDto == null || questionsDto.Count == 0)
            throw new Exception("Questions are missing");
        if (tagNames == null || tagNames.Count == 0)
            throw new Exception("Tags are missing");
    }

    private Survey MapSurvey(SurveyForCreatedDto surveyDto, List<QuestionForCreatedDto> questionsDto)
    {
        var survey = mapper.Map<SurveyForCreatedDto, Survey>(surveyDto);

        survey.Questions = mapper.Map<List<QuestionForCreatedDto>, List<Question>>(questionsDto);
        survey.CreatedDate = DateTime.UtcNow;

        return survey;
    }

    private async Task AssignTagsToSurvey(Survey survey, List<string> tagNames)
    {
        var existingTags = await repositoryManager.Tag.GetAllTags(true)
            .Where(tag => tagNames.Contains(tag.Name))
            .ToListAsync();

        var newTagNames = tagNames.Except(existingTags.Select(t => t.Name)).ToList();

        var newTags = newTagNames.Select(tagName => new Tag { Name = tagName }).ToList();
        existingTags.AddRange(newTags);

        survey.Tags = existingTags;
    }

    private async Task AssignTopicToSurvey(Survey survey, string topicName)
    {
        var topic = await repositoryManager.Topic.GetAllTopics(true)
            .FirstOrDefaultAsync(t => t.Name == topicName);

        if (topic == null)
            throw new Exception();

        survey.Topic = topic;
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
    
    private async Task AddSurveyForSearch(Survey survey)
    { 
        var surveyIndexDto = mapper.Map<Survey, SurveyForIndexDto>(survey);
        await surveySearchService.AddIndexAsync(surveyIndexDto);
    }

    private async Task<IQueryable<Survey>> GetAccessibleSurveys()
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