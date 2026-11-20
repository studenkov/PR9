using System.ComponentModel.DataAnnotations;

namespace PR9.Dtos;

public class UpdateCategoryDTO
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = null!;
}
