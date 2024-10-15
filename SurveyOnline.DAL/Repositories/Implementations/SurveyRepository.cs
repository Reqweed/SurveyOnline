using Microsoft.EntityFrameworkCore;
using SurveyOnline.DAL.Contexts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.DAL.Repositories.Implementations;

public class SurveyRepository(PostgresDbContext context) : BaseRepository<Survey>(context), ISurveyRepository
{
    public IQueryable<Survey> GetAllSurveys(bool trackChanges) => FindAll(trackChanges);

    public async Task<Survey?> GetSurveyByIdAsync(Guid idSurvey, bool trackChanges) =>
        await FindAll(trackChanges).Where(s => s.Id == idSurvey).SingleOrDefaultAsync();

    public async Task CreateSurveyAsync(Survey survey) => await CreateAsync(survey);

    public void UpdateSurvey(Survey survey) => Update(survey);

    public void DeleteSurvey(Survey survey) => Delete(survey);
}