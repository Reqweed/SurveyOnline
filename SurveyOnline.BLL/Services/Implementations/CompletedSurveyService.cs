using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SurveyOnline.BLL.Entities.DTOs.Survey;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.BLL.Services.Implementations;

public class CompletedSurveyService(SignInManager<User> signInManager, IRepositoryManager repositoryManager,
    IMapper mapper) : ICompletedSurveyService
{
    public async Task AddCompletedSurveyAsync(SurveyForCompletedDto surveyDto, List<string> answers)
    {
        ValidateInput(surveyDto, answers);

        var completedSurvey = await CreateCompletedSurveyAsync(answers, surveyDto);

        await repositoryManager.CompletedSurvey.CreateCompletedSurveyAsync(completedSurvey);
        await repositoryManager.SaveAsync();
    }

    private void ValidateInput(SurveyForCompletedDto surveyDto, List<string> answers)
    {
        if (surveyDto is null)
            throw new ArgumentNullException(nameof(surveyDto), "Survey cannot be null.");
        if (answers == null)
            throw new ArgumentNullException(nameof(answers), "Answers list cannot be null.");
        if (answers.Count != surveyDto.Questions.Count)
            throw new ArgumentException("The number of answers must match the number of questions.", nameof(answers));
    }

    private async Task<CompletedSurvey> CreateCompletedSurveyAsync(List<string> answers,
        SurveyForCompletedDto surveyDto)
    {
        var completedSurvey = new CompletedSurvey
        {
            SurveyId = surveyDto.Id
        };

        await AssignUserToCompletedSurveyAsync(completedSurvey);
        AssignAnswersToCompletedSurvey(completedSurvey, answers, surveyDto);

        return completedSurvey;
    }

    private void AssignAnswersToCompletedSurvey(CompletedSurvey completedSurvey, List<string> answers,
        SurveyForCompletedDto surveyDto)
    {
        var answersList = surveyDto.Questions
            .Select((t, i) => new Answer
            {
                AnswerValue = answers[i],
                QuestionId = t.Id
            })
            .ToList();

        completedSurvey.Answers = answersList;
    }

    private async Task AssignUserToCompletedSurveyAsync(CompletedSurvey completedSurvey)
    {
        var user = await signInManager.UserManager.GetUserAsync(signInManager.Context.User);
        if (user is null)
            throw new InvalidOperationException("User not found or not authenticated.");

        completedSurvey.User = user;
    }
}