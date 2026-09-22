using Microsoft.AspNetCore.Mvc;
using PR9.Data;
using PR9.DTOs;
using PR9.Models;

namespace PR9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly OnlineStoreContext _context;

    public CartController(OnlineStoreContext context)
    {
        _context = context;
    }

    private static CartDTO ToDto(Cart item) => new CartDTO
    {
        Id = item.Id,
        UserId = item.UserId,
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

        var items = _context.Carts
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
        var item = _context.Carts.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(ToDto(item));
    }

    [HttpPost]
    public IActionResult Create(CreateCartDTO dto)
    {
        var item = new Cart
        {
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Carts.Add(item);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, ToDto(item));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateCartDTO dto)
    {
        var item = _context.Carts.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        item.UserId = dto.UserId;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id, PatchCartDTO dto)
    {
        var item = _context.Carts.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        if (dto.UserId is not null)
        {
            item.UserId = dto.UserId.Value;
        }

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _context.Carts.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        _context.Carts.Remove(item);
        _context.SaveChanges();
        return NoContent();
    }
}
