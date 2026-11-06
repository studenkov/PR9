using Microsoft.AspNetCore.Mvc;
using PR9.DTOs;
using PR9.Services;

namespace PR9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    public ProductsController(IProductService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var (items, total) = await _service.GetAllAsync(page, pageSize, ct);
        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct) =>
        await _service.GetByIdAsync(id, ct) is { } item ? Ok(item) : NotFound();

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto, CancellationToken ct)
    {
        try { var result = await _service.CreateAsync(dto, ct); return CreatedAtAction(nameof(GetById), new { id = result.Id }, result); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto, CancellationToken ct)
    {
        try { var result = await _service.UpdateAsync(id, dto, ct); return result is null ? NotFound() : NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchProductDto dto, CancellationToken ct)
    {
        try { var result = await _service.PatchAsync(id, dto, ct); return result is null ? NotFound() : NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct) =>
        await _service.DeleteAsync(id, ct) ? NoContent() : NotFound();

    [HttpPost("{id:int}/reserve")]
    public async Task<IActionResult> Reserve(int id, ReserveProductDto dto, CancellationToken ct)
    {
        try { return Ok(await _service.ReserveProductAsync(id, dto.UserId, dto.Quantity, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("reservations/{reservationId:int}/cancel")]
    public async Task<IActionResult> CancelReservation(int reservationId, CancellationToken ct)
    {
        var result = await _service.CancelReservationAsync(reservationId, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
