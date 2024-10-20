namespace SurveyOnline.BLL.Entities.DTOs.Survey;

public record SurveyDto(Guid Id, string Title, string? UrlImage, string Description, string AuthorName);