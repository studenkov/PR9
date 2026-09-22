using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR9.Models;

public partial class User
{
    public int Id { get; set; }

    [NotMapped]
    public int IdUser
    {
        get => Id;
        set => Id = value;
    }

    public string LoginUser { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual Role? Role { get; set; }

    [NotMapped]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
