using System;

namespace PR9.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string LoginUser { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
