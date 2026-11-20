namespace PR9.Dtos;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public int ExpiresInMinutes { get; set; }
}
