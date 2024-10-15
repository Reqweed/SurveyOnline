using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.DAL.Repositories.Contracts;

public interface ITagRepository
{
    IQueryable<Tag> GetAllTags(bool trackChanges);
    Task<Tag?> GetTagByIdAsync(Guid idTag, bool trackChanges);
    Task CreateTagAsync(Tag tag);
    void UpdateTag(Tag tag);
    void DeleteTag(Tag tag);
}