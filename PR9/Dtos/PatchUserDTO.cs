namespace PR9.Dtos;

public class PatchUserDTO
{
    public string? LoginUser { get; set; }
    public string? PasswordHash { get; set; }
    public int? RoleId { get; set; }
    public bool? IsActive { get; set; }
}
