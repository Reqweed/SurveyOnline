using SurveyOnline.BLL.Entities.DTOs.Tag;

namespace SurveyOnline.BLL.Services.Contracts;

public interface ITagService
{
    IEnumerable<TagDto> GetAllTags();
}