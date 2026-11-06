using AutoMapper;
using PR9.DTOs;
using PR9.Models;
using PR9.Repositories;

namespace PR9.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemRepository _repository; private readonly IMapper _mapper;
    public OrderItemService(IOrderItemRepository repository, IMapper mapper) { _repository = repository; _mapper = mapper; }
    public async Task<(List<OrderItemDTO> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct) { page = Math.Max(1, page); pageSize = Math.Clamp(pageSize, 1, 100); return (_mapper.Map<List<OrderItemDTO>>(await _repository.GetAllAsync(page, pageSize, ct)), await _repository.CountAsync(ct)); }
    public async Task<OrderItemDTO?> GetByIdAsync(int id, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); return entity is null ? null : _mapper.Map<OrderItemDTO>(entity); }
    public async Task<OrderItemDTO> CreateAsync(CreateOrderItemDTO dto, CancellationToken ct) { await ValidateRelations(dto.OrderId, dto.ProductId, ct); var entity = _mapper.Map<OrderItem>(dto); entity.CreatedAt = DateTime.UtcNow; await _repository.AddAsync(entity, ct); await _repository.SaveChangesAsync(ct); return _mapper.Map<OrderItemDTO>(entity); }
    public async Task<bool> UpdateAsync(int id, UpdateOrderItemDTO dto, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); if (entity is null) return false; await ValidateRelations(dto.OrderId, dto.ProductId, ct); _mapper.Map(dto, entity); _repository.Update(entity); await _repository.SaveChangesAsync(ct); return true; }
    public async Task<bool> PatchAsync(int id, PatchOrderItemDTO dto, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); if (entity is null) return false; if (dto.OrderId is int oid && await _repository.FindOrderAsync(oid, ct) is null) throw Missing(oid); if (dto.ProductId is int pid && await _repository.FindProductAsync(pid, ct) is null) throw Missing(pid); if (dto.OrderId is int orderId) entity.OrderId = orderId; if (dto.ProductId is int productId) entity.ProductId = productId; if (dto.Quantity is int quantity) entity.Quantity = quantity; _repository.Update(entity); await _repository.SaveChangesAsync(ct); return true; }
    public async Task<bool> DeleteAsync(int id, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); if (entity is null) return false; _repository.Remove(entity); await _repository.SaveChangesAsync(ct); return true; }
    private async Task ValidateRelations(int orderId, int productId, CancellationToken ct) { if (await _repository.FindOrderAsync(orderId, ct) is null) throw Missing(orderId); if (await _repository.FindProductAsync(productId, ct) is null) throw Missing(productId); }
    private static InvalidOperationException Missing(int id) => new($"Связанная сущность с Id {id} не найдена.");
}
