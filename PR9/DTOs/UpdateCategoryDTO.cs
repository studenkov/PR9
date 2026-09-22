using System.ComponentModel.DataAnnotations;

namespace PR9.DTOs;

public class UpdateCategoryDTO
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = null!;
}
