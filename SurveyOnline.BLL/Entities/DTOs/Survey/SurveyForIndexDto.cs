using SurveyOnline.BLL.Entities.DTOs.Question;
using SurveyOnline.BLL.Entities.DTOs.Tag;
using SurveyOnline.BLL.Entities.DTOs.Topic;

namespace SurveyOnline.BLL.Entities.DTOs.Survey;

public record SurveyForIndexDto(
    Guid Id, 
    string Title, 
    string Description,
    string? UrlImage,
    string AuthorName,
    TopicDto Topic, 
    List<QuestionDto> Questions, 
    List<TagDto> Tags);