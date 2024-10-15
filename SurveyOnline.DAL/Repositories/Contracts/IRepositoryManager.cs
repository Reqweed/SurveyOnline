namespace SurveyOnline.DAL.Repositories.Contracts;

public interface IRepositoryManager
{
    ITagRepository Tag { get; }
    ISurveyRepository Survey { get; }
    ICompletedSurveyRepository CompletedSurvey { get; }
    ITopicRepository Topic { get; }
    Task SaveAsync();
}