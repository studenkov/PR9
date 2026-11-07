using System.ComponentModel.DataAnnotations;

namespace PR9.Dtos;

public class UpdateRoleDTO
{
    [Required]
    [StringLength(50)]
    public string Title { get; set; } = null!;
}
