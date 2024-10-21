using SurveyOnline.BLL.Entities.DTOs.Survey;

namespace SurveyOnline.BLL.Services.Contracts;

public interface ICompletedSurveyService
{
    Task AddCompletedSurvey(SurveyForCompletedDto surveyDto, List<string> answers);
}