using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PR9.Dtos;
using PR9.Models;
using PR9.Repositories;

namespace PR9.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly PasswordHasher<AppUser> _hasher = new();

    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AppUser> RegisterAsync(string login, string password, string role = "Customer", CancellationToken ct = default)
    {
        var exists = await _repository.ExistsByLoginAsync(login, ct);
        if (exists)
        {
            throw new InvalidOperationException($"Логин '{login}' уже занят.");
        }

        var normalizedRole = string.IsNullOrWhiteSpace(role) ? "Customer" : role;
        var user = new AppUser
        {
            Login = login,
            Role = normalizedRole,
            RoleId = string.Equals(normalizedRole, "Admin", StringComparison.OrdinalIgnoreCase) ? 1 : 2,
            IsActive = true
        };
        user.PasswordHash = _hasher.HashPassword(user, password);

        await _repository.AddAsync(user, ct);
        await _repository.SaveChangesAsync(ct);
        return user;
    }

    public async Task<AppUser?> ValidateCredentialsAsync(string login, string password, CancellationToken ct = default)
    {
        var user = await _repository.GetByLoginAsync(login, ct);
        if (user is null || !user.IsActive)
        {
            return null;
        }

        try
        {
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Failed ? null : user;
        }
        catch (FormatException)
        {
            return null;
        }
    }

    public async Task<(List<UserDTO> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var items = await _repository.GetAllAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<UserDTO>>(items), total);
    }

    public async Task<UserDTO?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        return entity is null ? null : _mapper.Map<UserDTO>(entity);
    }

    public async Task<UserDTO> CreateAsync(CreateUserDTO dto, CancellationToken ct)
    {
        var role = await _repository.FindRoleAsync(dto.RoleId, ct);
        if (role is null)
        {
            throw Missing(dto.RoleId);
        }

        var entity = _mapper.Map<AppUser>(dto);
        entity.Role = role.Title;
        entity.PasswordHash = _hasher.HashPassword(entity, dto.PasswordHash);
        await _repository.AddAsync(entity, ct);
        await _repository.SaveChangesAsync(ct);
        return _mapper.Map<UserDTO>(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserDTO dto, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return false;
        }

        var role = await _repository.FindRoleAsync(dto.RoleId, ct);
        if (role is null)
        {
            throw Missing(dto.RoleId);
        }

        _mapper.Map(dto, entity);
        entity.Role = role.Title;
        entity.PasswordHash = _hasher.HashPassword(entity, dto.PasswordHash);
        _repository.Update(entity);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> PatchAsync(int id, PatchUserDTO dto, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return false;
        }

        if (dto.RoleId is int roleId)
        {
            var role = await _repository.FindRoleAsync(roleId, ct);
            if (role is null)
            {
                throw Missing(roleId);
            }
            entity.RoleId = roleId;
            entity.Role = role.Title;
        }

        if (dto.LoginUser is not null)
        {
            entity.Login = dto.LoginUser;
        }
        if (dto.PasswordHash is not null)
        {
            entity.PasswordHash = _hasher.HashPassword(entity, dto.PasswordHash);
        }
        if (dto.IsActive is bool active)
        {
            entity.IsActive = active;
        }

        _repository.Update(entity);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return false;
        }

        _repository.Remove(entity);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    private static InvalidOperationException Missing(int id) =>
        new($"Связанная сущность с Id {id} не найдена.");
}
