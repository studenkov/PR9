using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR9.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    [NotMapped]
    public int IdOrderItem
    {
        get => Id;
        set => Id = value;
    }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    [NotMapped]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
