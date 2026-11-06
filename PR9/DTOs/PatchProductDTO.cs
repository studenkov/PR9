namespace PR9.DTOs;

public class PatchProductDto
{
    public string? Title { get; set; }
    public decimal? Price { get; set; }
    public int? CategoryId { get; set; }
    public string? ImagePath { get; set; }
    public int? StockQuantity { get; set; }
}

public class PatchProductDTO : PatchProductDto { }
