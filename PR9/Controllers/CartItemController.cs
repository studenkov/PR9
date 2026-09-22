using Microsoft.AspNetCore.Mvc;
using PR9.Data;
using PR9.DTOs;
using PR9.Models;

namespace PR9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartItemController : ControllerBase
{
    private readonly OnlineStoreContext _context;

    public CartItemController(OnlineStoreContext context)
    {
        _context = context;
    }

    private static CartItemDTO ToDto(CartItem item) => new CartItemDTO
    {
        Id = item.Id,
        CartId = item.CartId,
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

        var items = _context.CartItems
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
        var item = _context.CartItems.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(ToDto(item));
    }

    [HttpPost]
    public IActionResult Create(CreateCartItemDTO dto)
    {
        var item = new CartItem
        {
            CartId = dto.CartId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        _context.CartItems.Add(item);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, ToDto(item));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateCartItemDTO dto)
    {
        var item = _context.CartItems.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        item.CartId = dto.CartId;
        item.ProductId = dto.ProductId;
        item.Quantity = dto.Quantity;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id, PatchCartItemDTO dto)
    {
        var item = _context.CartItems.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        if (dto.CartId is not null)
        {
            item.CartId = dto.CartId.Value;
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
        var item = _context.CartItems.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        _context.CartItems.Remove(item);
        _context.SaveChanges();
        return NoContent();
    }
}
