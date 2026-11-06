using AutoMapper;
using PR9.DTOs;
using PR9.Models;
using PR9.Repositories;

namespace PR9.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository; private readonly IMapper _mapper;
    public CategoryService(ICategoryRepository repository, IMapper mapper) { _repository = repository; _mapper = mapper; }
    public async Task<(List<CategoryDTO> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct) { page = Math.Max(1, page); pageSize = Math.Clamp(pageSize, 1, 100); return (_mapper.Map<List<CategoryDTO>>(await _repository.GetAllAsync(page, pageSize, ct)), await _repository.CountAsync(ct)); }
    public async Task<CategoryDTO?> GetByIdAsync(int id, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); return entity is null ? null : _mapper.Map<CategoryDTO>(entity); }
    public async Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto, CancellationToken ct) { var entity = _mapper.Map<Category>(dto); entity.CreatedAt = DateTime.UtcNow; await _repository.AddAsync(entity, ct); await _repository.SaveChangesAsync(ct); return _mapper.Map<CategoryDTO>(entity); }
    public async Task<bool> UpdateAsync(int id, UpdateCategoryDTO dto, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); if (entity is null) return false; _mapper.Map(dto, entity); _repository.Update(entity); await _repository.SaveChangesAsync(ct); return true; }
    public async Task<bool> PatchAsync(int id, PatchCategoryDTO dto, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); if (entity is null) return false; if (dto.Title is not null) entity.Title = dto.Title; _repository.Update(entity); await _repository.SaveChangesAsync(ct); return true; }
    public async Task<bool> DeleteAsync(int id, CancellationToken ct) { var entity = await _repository.GetByIdAsync(id, ct); if (entity is null) return false; _repository.Remove(entity); await _repository.SaveChangesAsync(ct); return true; }
}
