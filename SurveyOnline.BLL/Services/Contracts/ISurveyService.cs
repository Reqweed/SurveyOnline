using SurveyOnline.BLL.Entities.DTOs.Question;
using SurveyOnline.BLL.Entities.DTOs.Survey;

namespace SurveyOnline.BLL.Services.Contracts;

public interface ISurveyService
{
    Task AddSurveyAsync(Guid idCreator, SurveyForCreatedDto surveyDto, List<QuestionForCreatedDto> questionsDto, List<Guid> idTags);
}