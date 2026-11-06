using PR9.Models;
namespace PR9.Repositories;
public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<Category?> GetByIdAsync(int id, CancellationToken ct);
    Task AddAsync(Category entity, CancellationToken ct);
    void Update(Category entity);
    void Remove(Category entity);
    Task SaveChangesAsync(CancellationToken ct);
}
