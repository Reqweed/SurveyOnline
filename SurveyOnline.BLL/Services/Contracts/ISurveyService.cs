using SurveyOnline.BLL.Entities.DTOs.Question;
using SurveyOnline.BLL.Entities.DTOs.Survey;

namespace SurveyOnline.BLL.Services.Contracts;

public interface ISurveyService
{
    Task CreateSurveyAsync(SurveyForCreatedDto surveyDto, List<QuestionForCreatedDto> questionsDto, List<string> tagNames, List<Guid> userIds);
    Task<IEnumerable<SurveyDto>> GetTopCompletedSurveysAsync(int countSurvey);
    Task<IEnumerable<SurveyDto>> GetPagedAccessibleSurveysAsync(int currentPage, int pageSize);
    Task<SurveyForCompletedDto> GetSurveyAsync(Guid idSurvey);
    Task<IEnumerable<SurveyDto>> GetSurveysByTag(string tag);
    Task<IEnumerable<SurveyDto>> GetSurveysByTerm(string term);
}