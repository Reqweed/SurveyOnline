using SurveyOnline.BLL.Entities.DTOs.Survey;

namespace SurveyOnline.BLL.Services.Contracts;

public interface ICompletedSurveyService
{
    Task AddCompletedSurveyAsync(SurveyForCompletedDto surveyDto, List<string> answers);
}