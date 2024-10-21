using SurveyOnline.BLL.Entities.DTOs.Question;

namespace SurveyOnline.BLL.Entities.DTOs.Answer;

public record AnswerDto(string AnswerValue, QuestionDto Question);