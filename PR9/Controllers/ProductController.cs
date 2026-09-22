using Microsoft.AspNetCore.Mvc;
using PR9.Data;
using PR9.DTOs;
using PR9.Models;

namespace PR9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly OnlineStoreContext _context;

    public ProductController(OnlineStoreContext context)
    {
        _context = context;
    }

    private static ProductDTO ToDto(Product item) => new ProductDTO
    {
        Id = item.Id,
        Title = item.Title,
        Price = item.Price,
        CategoryId = item.CategoryId,
        ImagePath = item.ImagePath,
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

        var items = _context.Products
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
        var item = _context.Products.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(ToDto(item));
    }

    [HttpPost]
    public IActionResult Create(CreateProductDTO dto)
    {
        var item = new Product
        {
            Title = dto.Title,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            ImagePath = dto.ImagePath,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(item);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, ToDto(item));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateProductDTO dto)
    {
        var item = _context.Products.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        item.Title = dto.Title;
        item.Price = dto.Price;
        item.CategoryId = dto.CategoryId;
        item.ImagePath = dto.ImagePath;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id, PatchProductDTO dto)
    {
        var item = _context.Products.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        if (dto.Title is not null)
        {
            item.Title = dto.Title;
        }

        if (dto.Price is not null)
        {
            item.Price = dto.Price.Value;
        }

        if (dto.CategoryId is not null)
        {
            item.CategoryId = dto.CategoryId.Value;
        }

        if (dto.ImagePath is not null)
        {
            item.ImagePath = dto.ImagePath;
        }

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _context.Products.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        _context.Products.Remove(item);
        _context.SaveChanges();
        return NoContent();
    }
}
