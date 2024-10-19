using SurveyOnline.BLL.Entities.DTOs.Question;
using SurveyOnline.BLL.Entities.DTOs.Survey;

namespace SurveyOnline.BLL.Services.Contracts;

public interface ISurveyService
{
    Task CreateSurveyAsync(Guid idCreator, SurveyForCreatedDto surveyDto, List<QuestionForCreatedDto> questionsDto, List<Guid> tagIds, List<Guid> userIds);
    Task<IEnumerable<SurveyDto>> GetTopCompletedSurveysAsync(Guid idUser, int countSurvey);
    Task<IEnumerable<SurveyDto>> GetPagedAccessibleSurveysAsync(Guid idUser, int currentPage, int pageSize);
}