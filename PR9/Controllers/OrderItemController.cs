using Microsoft.AspNetCore.Mvc;
using PR9.Data;
using PR9.DTOs;
using PR9.Models;

namespace PR9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderItemController : ControllerBase
{
    private readonly OnlineStoreContext _context;

    public OrderItemController(OnlineStoreContext context)
    {
        _context = context;
    }

    private static OrderItemDTO ToDto(OrderItem item) => new OrderItemDTO
    {
        Id = item.Id,
        OrderId = item.OrderId,
        ProductId = item.ProductId,
        Quantity = item.Quantity,
        CreatedAt = item.CreatedAt
    };

    [HttpGet]
    public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 10;
        }

        var items = _context.OrderItems
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList()
            .Select(ToDto)
            .ToList();

        return Ok(items);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var item = _context.OrderItems.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(ToDto(item));
    }

    [HttpPost]
    public IActionResult Create(CreateOrderItemDTO dto)
    {
        var item = new OrderItem
        {
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        _context.OrderItems.Add(item);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, ToDto(item));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateOrderItemDTO dto)
    {
        var item = _context.OrderItems.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        item.OrderId = dto.OrderId;
        item.ProductId = dto.ProductId;
        item.Quantity = dto.Quantity;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id, PatchOrderItemDTO dto)
    {
        var item = _context.OrderItems.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        if (dto.OrderId is not null)
        {
            item.OrderId = dto.OrderId.Value;
        }

        if (dto.ProductId is not null)
        {
            item.ProductId = dto.ProductId.Value;
        }

        if (dto.Quantity is not null)
        {
            item.Quantity = dto.Quantity.Value;
        }

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _context.OrderItems.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        _context.OrderItems.Remove(item);
        _context.SaveChanges();
        return NoContent();
    }
}
