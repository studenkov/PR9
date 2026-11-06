using Microsoft.AspNetCore.Mvc;
using PR9.DTOs;
using PR9.Services;

namespace PR9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderItemController : ControllerBase
{
    private readonly IOrderItemService _service;
    public OrderItemController(IOrderItemService service) => _service = service;
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default) { var (items, total) = await _service.GetAllAsync(page, pageSize, ct); Response.Headers["X-Total-Count"] = total.ToString(); return Ok(items); }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct) => await _service.GetByIdAsync(id, ct) is { } item ? Ok(item) : NotFound();
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderItemDTO dto, CancellationToken ct) { try { var item = await _service.CreateAsync(dto, ct); return CreatedAtAction(nameof(GetById), new { id = item.Id }, item); } catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); } }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateOrderItemDTO dto, CancellationToken ct) { try { return await _service.UpdateAsync(id, dto, ct) ? NoContent() : NotFound(); } catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); } }
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchOrderItemDTO dto, CancellationToken ct) { try { return await _service.PatchAsync(id, dto, ct) ? NoContent() : NotFound(); } catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); } }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct) => await _service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
