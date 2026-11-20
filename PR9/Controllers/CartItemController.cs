using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PR9.Dtos;
using PR9.Services;

namespace PR9.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartItemController : ControllerBase
{
    private readonly ICartItemService _service;
    public CartItemController(ICartItemService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        int? userId = null;
        if (!User.IsInRole("Admin"))
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var currentUserId))
                return Unauthorized();
            userId = currentUserId;
        }

        var (items, total) = await _service.GetAllAsync(page, pageSize, userId, ct);
        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        if (!User.IsInRole("Admin"))
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var currentUserId) ||
                !await _service.CanAccessCartItemAsync(id, currentUserId, ct))
                return Forbid();
        }

        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCartItemDTO dto, CancellationToken ct)
    {
        if (!User.IsInRole("Admin"))
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var currentUserId) ||
                !await _service.CanAccessCartAsync(dto.CartId, currentUserId, ct))
                return Forbid();
        }

        try
        {
            var item = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateCartItemDTO dto, CancellationToken ct)
    {
        if (!User.IsInRole("Admin"))
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var currentUserId) ||
                !await _service.CanAccessCartItemAsync(id, currentUserId, ct) ||
                !await _service.CanAccessCartAsync(dto.CartId, currentUserId, ct))
                return Forbid();
        }

        try
        {
            return await _service.UpdateAsync(id, dto, ct) ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchCartItemDTO dto, CancellationToken ct)
    {
        if (!User.IsInRole("Admin"))
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var currentUserId) ||
                !await _service.CanAccessCartItemAsync(id, currentUserId, ct) ||
                (dto.CartId.HasValue && !await _service.CanAccessCartAsync(dto.CartId.Value, currentUserId, ct)))
                return Forbid();
        }

        try
        {
            return await _service.PatchAsync(id, dto, ct) ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        if (!User.IsInRole("Admin"))
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var currentUserId) ||
                !await _service.CanAccessCartItemAsync(id, currentUserId, ct))
                return Forbid();
        }

        return await _service.DeleteAsync(id, ct) ? NoContent() : NotFound();
    }
}
