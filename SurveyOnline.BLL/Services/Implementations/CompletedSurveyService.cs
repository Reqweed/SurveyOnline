using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SurveyOnline.BLL.Entities.DTOs.Survey;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.BLL.Services.Implementations;

public class CompletedSurveyService(SignInManager<User> signInManager, IRepositoryManager repositoryManager, IMapper mapper) : ICompletedSurveyService
{
    public async Task AddCompletedSurvey(SurveyForCompletedDto surveyDto, List<string> answers)
    {
        if (surveyDto is null)
            throw new Exception();
        if (answers.Count != surveyDto.Questions.Count)
            throw new Exception();

        var completedSurvey = new CompletedSurvey();
        completedSurvey.SurveyId = surveyDto.Id;
        var user =  await signInManager.UserManager.GetUserAsync(signInManager.Context.User);
        if (user is null)
            throw new Exception();
        completedSurvey.User = user;
        var answersList = surveyDto.Questions.Select((t, i) => new Answer { AnswerValue = answers[i], QuestionId = t.Id }).ToList();

        completedSurvey.Answers = answersList;

        await repositoryManager.CompletedSurvey.CreateCompletedSurveyAsync(completedSurvey);
        await repositoryManager.SaveAsync();
    }
}