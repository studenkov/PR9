namespace PR9.Models;

public partial class AppUser
{
    public int Id { get; set; }

    public string Login { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "Customer";

    public bool IsActive { get; set; } = true;

    public int? RoleId { get; set; }

    public virtual Role? RoleNavigation { get; set; }

    public virtual ICollection<ProductReservation> ProductReservations { get; set; } = new List<ProductReservation>();
}
