namespace HortaGestao.Application.DTOs.Authentication;

public class UserAspNetDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public List<string> Role { get; set; }
    
}