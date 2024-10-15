using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.DAL.Repositories.Contracts;

public interface ICompletedSurveyRepository
{
    IQueryable<CompletedSurvey> GetAllCompletedSurveys(bool trackChanges);
    Task<CompletedSurvey?> GetCompletedSurveyByIdAsync(Guid idCompletedSurvey, bool trackChanges);
    Task CreateCompletedSurveyAsync(CompletedSurvey completedSurvey);
    void UpdateCompletedSurvey(CompletedSurvey completedSurvey);
    void DeleteCompletedSurvey(CompletedSurvey completedSurvey);
}