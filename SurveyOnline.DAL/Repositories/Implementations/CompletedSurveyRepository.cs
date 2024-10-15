using Microsoft.EntityFrameworkCore;
using SurveyOnline.DAL.Contexts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.DAL.Repositories.Implementations;

public class CompletedSurveyRepository(PostgresDbContext context) : BaseRepository<CompletedSurvey>(context),
    ICompletedSurveyRepository
{
    public IQueryable<CompletedSurvey> GetAllCompletedSurveys(bool trackChanges) => FindAll(trackChanges);

    public async Task<CompletedSurvey?> GetCompletedSurveyByIdAsync(Guid idCompletedSurvey, bool trackChanges)
        => await FindAll(trackChanges).Where(completedSurvey => completedSurvey.Id == idCompletedSurvey)
            .FirstOrDefaultAsync();

    public Task CreateCompletedSurveyAsync(CompletedSurvey completedSurvey) => CreateAsync(completedSurvey);

    public void UpdateCompletedSurvey(CompletedSurvey completedSurvey) => Update(completedSurvey);

    public void DeleteCompletedSurvey(CompletedSurvey completedSurvey) => Delete(completedSurvey);
}