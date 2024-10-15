using Microsoft.EntityFrameworkCore;
using SurveyOnline.DAL.Contexts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.DAL.Repositories.Implementations;

public class TopicRepository(PostgresDbContext context) : BaseRepository<Topic>(context), ITopicRepository
{
    public IQueryable<Topic> GetAllTopics(bool trackChanges) => FindAll(trackChanges);

    public async Task<Topic?> GetTopicByIdAsync(Guid idTopic, bool trackChanges) =>
        await FindAll(trackChanges).Where(topic => topic.Id == idTopic).FirstOrDefaultAsync();

    public async Task CreateTopicAsync(Topic topic) => await CreateAsync(topic);

    public void UpdateTopic(Topic topic) => Update(topic);

    public void DeleteTopic(Topic topic) => Delete(topic);
}