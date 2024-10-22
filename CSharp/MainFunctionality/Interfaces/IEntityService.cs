namespace FormsClone.CSharp.MainFunctionality.Interfaces
{
    public interface IEntityService<T>
    {
        Task CreateAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }

}
