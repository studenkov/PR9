using Microsoft.AspNetCore.Mvc;
using PR9.Data;
using PR9.DTOs;
using PR9.Models;

namespace PR9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly OnlineStoreContext _context;

    public UserController(OnlineStoreContext context)
    {
        _context = context;
    }

    private static UserDTO ToDto(User item) => new UserDTO
    {
        Id = item.Id,
        LoginUser = item.LoginUser,
        PasswordHash = item.PasswordHash,
        RoleId = item.RoleId,
        IsActive = item.IsActive,
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

        var items = _context.Users
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
        var item = _context.Users.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(ToDto(item));
    }

    [HttpPost]
    public IActionResult Create(CreateUserDTO dto)
    {
        var item = new User
        {
            LoginUser = dto.LoginUser,
            PasswordHash = dto.PasswordHash,
            RoleId = dto.RoleId,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(item);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, ToDto(item));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateUserDTO dto)
    {
        var item = _context.Users.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        item.LoginUser = dto.LoginUser;
        item.PasswordHash = dto.PasswordHash;
        item.RoleId = dto.RoleId;
        item.IsActive = dto.IsActive;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id, PatchUserDTO dto)
    {
        var item = _context.Users.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        if (dto.LoginUser is not null)
        {
            item.LoginUser = dto.LoginUser;
        }

        if (dto.PasswordHash is not null)
        {
            item.PasswordHash = dto.PasswordHash;
        }

        if (dto.RoleId is not null)
        {
            item.RoleId = dto.RoleId.Value;
        }

        if (dto.IsActive is not null)
        {
            item.IsActive = dto.IsActive.Value;
        }

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _context.Users.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        _context.Users.Remove(item);
        _context.SaveChanges();
        return NoContent();
    }
}
