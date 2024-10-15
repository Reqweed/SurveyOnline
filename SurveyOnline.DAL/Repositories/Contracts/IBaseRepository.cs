namespace SurveyOnline.DAL.Repositories.Contracts;

public interface IBaseRepository<T>
{
    IQueryable<T> FindAll(bool trackChanges);
    Task CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}