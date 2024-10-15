using Microsoft.EntityFrameworkCore;
using SurveyOnline.DAL.Contexts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.DAL.Repositories.Implementations;

public class TagRepository(PostgresDbContext context) : BaseRepository<Tag>(context), ITagRepository
{
    public IQueryable<Tag> GetAllTags(bool trackChanges) => FindAll(trackChanges);

    public async Task<Tag?> GetTagByIdAsync(Guid idTag, bool trackChanges) =>
        await FindAll(trackChanges).Where(s => s.Id == idTag).SingleOrDefaultAsync();

    public async Task CreateTagAsync(Tag tag) => await CreateAsync(tag);

    public void UpdateTag(Tag tag) => Update(tag);

    public void DeleteTag(Tag tag) => Delete(tag);
}