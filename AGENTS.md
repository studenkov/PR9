# AGENTS.md — Architectural & Coding Standards for PR9 Web API

## 1. Context & Architecture Overview

This is an ASP.NET Core Web API project (namespace: `PR9`) connected to a real database via Entity Framework Core.
You must adhere strictly to a 3-tier Layered Architecture:
`Controllers` -> `Services` -> `Repositories` -> `Database (DbContext)`

### Architectural Invariants (DO NOT VIOLATE):

- **NO DbContext outside Repositories**: Neither Controllers nor Services may inject or reference `DbContext` / `AppDbContext`. All data access goes through dedicated repository interfaces.
- **NO HTTP logic in Services**: Services must never import or use `Microsoft.AspNetCore.Mvc`, `IActionResult`, `StatusCodes`, or HTTP headers. Business errors are communicated by returning domain outcomes/null or throwing `InvalidOperationException`.
- **NO Generic Repositories / NO UnitOfWork**: Do not create generic `Repository<T>` or `UnitOfWork` classes. Implement explicit, strongly typed repositories (e.g. `ProductRepository`, `UserRepository`).
- **NO Explicit DB Transactions**: Do not use `Database.BeginTransactionAsync()`. Multiple entity modifications within a single business operation must be persisted using a single call to `SaveChangesAsync()`.
- **Lifetime**: All Repositories and Services MUST be registered in DI as `Scoped` (`builder.Services.AddScoped<TInterface, TImpl>()`).

---

## 2. Solution Structure & Directories

Reorganize and place files strictly into the following folder structure:

- `PR9/Models/` — EF Core entity models (`Product`, `Category`, `User`, `Role`, `Order`, `OrderItem`, `Cart`, `CartItem`, `ProductReservation`, and `AppDbContext`).
- `PR9/Dtos/` — Separate DTOs per entity:
  - `{Entity}Dto.cs` (Read, contains nested related DTOs, e.g. `CategoryDto` inside `ProductDto`).
  - `Create{Entity}Dto.cs` (Write/POST, accepts foreign keys as IDs).
  - `Update{Entity}Dto.cs` (Full update/PUT, non-nullable required fields).
  - `Patch{Entity}Dto.cs` (Partial update/PATCH, all properties are nullable).
- `PR9/Mapping/` — AutoMapper profile `MappingProfile.cs`.
- `PR9/Repositories/` — Interfaces (`IProductRepository`, etc.) and implementations (`ProductRepository`, etc.).
- `PR9/Services/` — Interfaces (`IProductService`, etc.) and implementations (`ProductService`, etc.).
- `PR9/Controllers/` — Thin API controllers (`ProductsController`, etc.).

---

## 3. Layer Implementation Rules

### Repositories (`PR9.Repositories`)

- Methods must support `CancellationToken ct`.
- List queries must support pagination: `int page, int pageSize`.
- Must include navigation properties using `.Include()`.
- Must provide a `CountAsync(ct)` method for pagination totals.
- Must provide single `Task SaveChangesAsync(CancellationToken ct)`.

### Services (`PR9.Services`)

- Inject corresponding `IRepository` and `AutoMapper.IMapper`.
- Pagination method signature: `Task<(List<ProductDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct)`.
- Creation/Update must validate related foreign keys via repository lookup; if missing, throw `InvalidOperationException($"Связанная сущность с Id {id} не найдена.")`.
- Patch must update only non-null fields from `PatchDto`.

### Controllers (`PR9.Controllers`)

- Attribute `[ApiController]`, route `[Route("api/[controller]")]`.
- In `GetAll()`: set response header `Response.Headers["X-Total-Count"] = total.ToString();` and return `Ok(items)`.
- In `GetById(id)`: return `NotFound()` if null, else `Ok(dto)`.
- In `Create(dto)`: call `CreatedAtAction(nameof(GetById), new { id = result.Id }, result)`. Catch `InvalidOperationException` and return `BadRequest(new { error = ex.Message })`.
- In `Update(id, dto)` and `Patch(id, dto)`: return `NotFound()` if entity not found. Catch `InvalidOperationException` and return `BadRequest(new { error = ex.Message })`.
- In `Delete(id)`: return `NoContent()` if success, `NotFound()` if false.

---

## 4. Business Operation (Resource Reservation & Rollback)

For the required dual-entity modification:

1. **New Event Entity**: `ProductReservation` (`Id`, `ProductId`, `Product`, `UserId`, `User`, `Quantity`, `ReservedAt`, `IsActive`).
2. **Resource Property**: Ensure `Product` has `StockQuantity` (int) to represent available stock.
3. **Reserve Operation**:
   - Verify `Product` exists and `product.StockQuantity >= quantity`.
   - If not enough stock: throw `InvalidOperationException("Недостаточно товара на складе.")` -> controller maps to 400.
   - Decrement `product.StockQuantity -= quantity`.
   - Add new `ProductReservation` with `IsActive = true`.
   - Persist both changes via a **single** `await _repository.SaveChangesAsync(ct)`.
4. **Cancel/Rollback Operation**:
   - Find active `ProductReservation` by Id. If not found or `!IsActive`, return null -> controller maps to 404.
   - Revert stock: `product.StockQuantity += reservation.Quantity`.
   - Mark `reservation.IsActive = false`.
   - Persist both changes via a **single** `await _repository.SaveChangesAsync(ct)`.
