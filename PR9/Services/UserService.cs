using AutoMapper;
using PR9.DTOs;
using PR9.Models;
using PR9.Repositories;

namespace PR9.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository; private readonly IMapper _mapper;
    public UserService(IUserRepository repository, IMapper mapper) { _repository = repository; _mapper = mapper; }
    public async Task<(List<UserDTO> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct) { page = Math.Max(1, page); pageSize = Math.Clamp(pageSize, 1, 100); return (_mapper.Map<List<UserDTO>>(await _repository.GetAllAsync(page, pageSize, ct)), await _repository.CountAsync(ct)); }
    public async Task<UserDTO?> GetByIdAsync(int id, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); return entity is null ? null : _mapper.Map<UserDTO>(entity); }
    public async Task<UserDTO> CreateAsync(CreateUserDTO dto, CancellationToken ct) { if (await _repository.FindRoleAsync(dto.RoleId, ct) is null) throw Missing(dto.RoleId); var entity = _mapper.Map<User>(dto); entity.CreatedAt = DateTime.UtcNow; await _repository.AddAsync(entity, ct); await _repository.SaveChangesAsync(ct); return _mapper.Map<UserDTO>(entity); }
    public async Task<bool> UpdateAsync(int id, UpdateUserDTO dto, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); if (entity is null) return false; if (await _repository.FindRoleAsync(dto.RoleId, ct) is null) throw Missing(dto.RoleId); _mapper.Map(dto, entity); _repository.Update(entity); await _repository.SaveChangesAsync(ct); return true; }
    public async Task<bool> PatchAsync(int id, PatchUserDTO dto, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); if (entity is null) return false; if (dto.RoleId is int roleId && await _repository.FindRoleAsync(roleId, ct) is null) throw Missing(roleId); if (dto.LoginUser is not null) entity.LoginUser = dto.LoginUser; if (dto.PasswordHash is not null) entity.PasswordHash = dto.PasswordHash; if (dto.RoleId is int rid) entity.RoleId = rid; if (dto.IsActive is bool active) entity.IsActive = active; _repository.Update(entity); await _repository.SaveChangesAsync(ct); return true; }
    public async Task<bool> DeleteAsync(int id, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); if (entity is null) return false; _repository.Remove(entity); await _repository.SaveChangesAsync(ct); return true; }
    private static InvalidOperationException Missing(int id) => new($"Связанная сущность с Id {id} не найдена.");
}
