using SurveyOnline.DAL.Entities.Enums;

namespace SurveyOnline.BLL.Entities.DTOs.Question;

public record QuestionDto(Guid Id, string Title, string Description, QuestionType Type);