using System.ComponentModel.DataAnnotations;

namespace PR9.Dtos;

public class CreateCategoryDTO
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = null!;
}

public class CreateCategoryDto : CreateCategoryDTO { }
