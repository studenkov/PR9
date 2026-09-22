using Microsoft.AspNetCore.Mvc;
using PR9.Data;
using PR9.DTOs;
using PR9.Models;

namespace PR9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OnlineStoreContext _context;

    public OrderController(OnlineStoreContext context)
    {
        _context = context;
    }

    private static OrderDTO ToDto(Order item) => new OrderDTO
    {
        Id = item.Id,
        UserId = item.UserId,
        DateOrder = item.DateOrder,
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

        var items = _context.Orders
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
        var item = _context.Orders.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(ToDto(item));
    }

    [HttpPost]
    public IActionResult Create(CreateOrderDTO dto)
    {
        var item = new Order
        {
            UserId = dto.UserId,
            DateOrder = dto.DateOrder,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(item);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, ToDto(item));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateOrderDTO dto)
    {
        var item = _context.Orders.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        item.UserId = dto.UserId;
        item.DateOrder = dto.DateOrder;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id, PatchOrderDTO dto)
    {
        var item = _context.Orders.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        if (dto.UserId is not null)
        {
            item.UserId = dto.UserId.Value;
        }

        if (dto.DateOrder is not null)
        {
            item.DateOrder = dto.DateOrder.Value;
        }

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _context.Orders.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        _context.Orders.Remove(item);
        _context.SaveChanges();
        return NoContent();
    }
}
