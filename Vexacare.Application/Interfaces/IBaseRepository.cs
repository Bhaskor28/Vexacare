namespace Vexacare.Application.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        //public IQueryable<T> Table { get; }
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
    }
}
