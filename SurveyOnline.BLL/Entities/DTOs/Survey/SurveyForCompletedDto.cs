using SurveyOnline.BLL.Entities.DTOs.Question;
using SurveyOnline.BLL.Entities.DTOs.Tag;

namespace SurveyOnline.BLL.Entities.DTOs.Survey;

public record SurveyForCompletedDto(
    Guid Id, 
    string Title, 
    string Description, 
    string? UrlImage, 
    string TopicName,
    string Creator, 
    List<QuestionDto> Questions, 
    List<TagDto> Tags);