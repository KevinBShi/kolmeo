using ProductAPI.Models;

namespace ProductAPI.Repositories
{
    public interface IRepository<TModel> where TModel : BaseEntity
    {
        Task<IList<TModel>> GetAllAsync();
        Task<TModel?> GetByIdAsync(int id);
        Task CreateAsync(TModel item);
        Task UpdateAsync(int id, TModel item);
        Task DeleteAsync(int id);
    }
}
