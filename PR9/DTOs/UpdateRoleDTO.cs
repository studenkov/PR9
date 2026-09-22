using System.ComponentModel.DataAnnotations;

namespace PR9.DTOs;

public class UpdateRoleDTO
{
    [Required]
    [StringLength(50)]
    public string Title { get; set; } = null!;
}
