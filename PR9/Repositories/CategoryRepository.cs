using Microsoft.EntityFrameworkCore;
using PR9.Models;

namespace PR9.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;
    public CategoryRepository(AppDbContext context) => _context = context;
    public Task<List<Category>> GetAllAsync(int page, int pageSize, CancellationToken ct) => _context.Categories.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct) => _context.Categories.CountAsync(ct);
    public Task<Category?> GetByIdAsync(int id, CancellationToken ct) => _context.Categories.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(Category entity, CancellationToken ct) => _context.Categories.AddAsync(entity, ct).AsTask();
    public void Update(Category entity) => _context.Categories.Update(entity);
    public void Remove(Category entity) => _context.Categories.Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}