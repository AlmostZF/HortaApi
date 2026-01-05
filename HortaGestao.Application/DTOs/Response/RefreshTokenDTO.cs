namespace HortaGestao.Application.DTOs.Response;

public class RefreshTokenDto
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; }
}