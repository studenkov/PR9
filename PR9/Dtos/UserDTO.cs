using System;

namespace PR9.Dtos;

public class UserDTO
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string LoginUser
    {
        get => Login;
        set => Login = value;
    }
    public string Role { get; set; } = "Customer";
    public int RoleId { get; set; } = 2;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
