using SurveyOnline.DAL.Contexts;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.DAL.Repositories.Implementations;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<ITagRepository> _tagRepository;
    private readonly Lazy<ISurveyRepository> _surveyRepository;
    private readonly Lazy<ICompletedSurveyRepository> _completedSurveyRepository;
    private readonly Lazy<ITopicRepository> _topicRepository;
    private readonly PostgresDbContext _context;

    public RepositoryManager(PostgresDbContext context)
    {
        _context = context;
        _tagRepository = new Lazy<ITagRepository>(() => new TagRepository(_context));
        _surveyRepository = new Lazy<ISurveyRepository>(() => new SurveyRepository(_context));
        _completedSurveyRepository = new Lazy<ICompletedSurveyRepository>(() => new CompletedSurveyRepository(context));
        _topicRepository = new Lazy<ITopicRepository>(() => new TopicRepository(context));
    }

    public ITagRepository Tag => _tagRepository.Value;
    public ISurveyRepository Survey => _surveyRepository.Value;
    public ICompletedSurveyRepository CompletedSurvey => _completedSurveyRepository.Value;
    public ITopicRepository Topic => _topicRepository.Value;

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}