namespace DDDPractice.Application.DTOs.Interface;

public class AuthUserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string? Token { get; set; }
    public DateTime? Expiration { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}