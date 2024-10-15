using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.DAL.Repositories.Contracts;

public interface ISurveyRepository
{
    IQueryable<Survey> GetAllSurveys(bool trackChanges);
    Task<Survey?> GetSurveyByIdAsync(Guid idSurvey, bool trackChanges);
    Task CreateSurveyAsync(Survey survey);
    void UpdateSurvey(Survey survey);
    void DeleteSurvey(Survey survey);
}