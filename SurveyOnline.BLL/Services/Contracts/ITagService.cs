using SurveyOnline.BLL.Entities.DTOs.Tag;

namespace SurveyOnline.BLL.Services.Contracts;

public interface ITagService
{
    Task<IEnumerable<TagDto>> GetAllTagsAsync();
    Task<IEnumerable<TagForCloudDto>> GetAllTagsForCloudAsync();
}