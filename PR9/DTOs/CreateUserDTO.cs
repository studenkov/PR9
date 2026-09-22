using System.ComponentModel.DataAnnotations;

namespace PR9.DTOs;

public class CreateUserDTO
{
    [Required]
    [StringLength(50)]
    public string LoginUser { get; set; } = null!;

    [Required]
    [StringLength(256)]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public int RoleId { get; set; }

    public bool IsActive { get; set; } = true;
}
