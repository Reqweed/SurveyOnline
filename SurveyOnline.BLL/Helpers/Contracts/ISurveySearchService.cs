using SurveyOnline.BLL.Entities.DTOs.Survey;

namespace SurveyOnline.BLL.Helpers.Contracts;

public interface ISurveySearchService
{
    Task AddIndexAsync(SurveyForIndexDto surveyDto);
    Task<IEnumerable<SurveyForIndexDto>> SearchSurveyByTagAsync(string tagName);
    Task<IEnumerable<SurveyForIndexDto>> SearchSurveysAsync(string searchTerm);
}