using Microsoft.EntityFrameworkCore;
using PR9.Models;

namespace PR9.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;
    public RoleRepository(AppDbContext context) => _context = context;
    public Task<List<Role>> GetAllAsync(int page, int pageSize, CancellationToken ct) => _context.Roles.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct) => _context.Roles.CountAsync(ct);
    public Task<Role?> GetByIdAsync(int id, CancellationToken ct) => _context.Roles.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(Role entity, CancellationToken ct) => _context.Roles.AddAsync(entity, ct).AsTask();
    public void Update(Role entity) => _context.Roles.Update(entity);
    public void Remove(Role entity) => _context.Roles.Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}