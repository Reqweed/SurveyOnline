using Microsoft.EntityFrameworkCore;
using SurveyOnline.DAL.Contexts;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.DAL.Repositories.Implementations;

public abstract class BaseRepository<T>(PostgresDbContext context) : IBaseRepository<T> where T : class
{
    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges
            ? context.Set<T>().AsNoTracking()
            : context.Set<T>();

    public async Task CreateAsync(T entity) => await context.Set<T>().AddAsync(entity);

    public void Update(T entity) => context.Set<T>().Update(entity);

    public void Delete(T entity) => context.Set<T>().Remove(entity);
}