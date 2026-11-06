using AutoMapper;
using PR9.DTOs;
using PR9.Models;
using PR9.Repositories;

namespace PR9.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository; private readonly IMapper _mapper;
    public OrderService(IOrderRepository repository, IMapper mapper) { _repository = repository; _mapper = mapper; }
    public async Task<(List<OrderDTO> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct) { page = Math.Max(1, page); pageSize = Math.Clamp(pageSize, 1, 100); return (_mapper.Map<List<OrderDTO>>(await _repository.GetAllAsync(page, pageSize, ct)), await _repository.CountAsync(ct)); }
    public async Task<OrderDTO?> GetByIdAsync(int id, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); return entity is null ? null : _mapper.Map<OrderDTO>(entity); }
    public async Task<OrderDTO> CreateAsync(CreateOrderDTO dto, CancellationToken ct) { if (await _repository.FindUserAsync(dto.UserId, ct) is null) throw Missing(dto.UserId); var entity = _mapper.Map<Order>(dto); entity.CreatedAt = DateTime.UtcNow; await _repository.AddAsync(entity, ct); await _repository.SaveChangesAsync(ct); return _mapper.Map<OrderDTO>(entity); }
    public async Task<bool> UpdateAsync(int id, UpdateOrderDTO dto, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); if (entity is null) return false; if (await _repository.FindUserAsync(dto.UserId, ct) is null) throw Missing(dto.UserId); _mapper.Map(dto, entity); _repository.Update(entity); await _repository.SaveChangesAsync(ct); return true; }
    public async Task<bool> PatchAsync(int id, PatchOrderDTO dto, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); if (entity is null) return false; if (dto.UserId is int uid && await _repository.FindUserAsync(uid, ct) is null) throw Missing(uid); if (dto.UserId is int userId) entity.UserId = userId; if (dto.DateOrder is DateOnly date) entity.DateOrder = date; _repository.Update(entity); await _repository.SaveChangesAsync(ct); return true; }
    public async Task<bool> DeleteAsync(int id, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); if (entity is null) return false; _repository.Remove(entity); await _repository.SaveChangesAsync(ct); return true; }
    private static InvalidOperationException Missing(int id) => new($"Связанная сущность с Id {id} не найдена.");
}
