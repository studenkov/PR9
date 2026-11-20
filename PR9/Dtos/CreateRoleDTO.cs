using System.ComponentModel.DataAnnotations;

namespace PR9.Dtos;

public class CreateRoleDTO
{
    [Required]
    [StringLength(50)]
    public string Title { get; set; } = null!;
}
