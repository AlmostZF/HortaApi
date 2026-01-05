namespace HortaGestao.Domain.Entities;


public class RefreshTokenEntity
{
    public Guid Id { get; set; }
    
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime Expires { get; set; }
    public DateTime AbsoluteExpires { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime Created { get; set; }
    public string? ReplacedByToken { get; set; }
    
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsAbsoluteExpired => DateTime.UtcNow >= AbsoluteExpires;
    public bool IsActive => !IsRevoked && !IsExpired && !IsAbsoluteExpired;

    public void Revoke(string reason, string replacedByToken = null)
    {
        IsRevoked = true;
        ReplacedByToken = replacedByToken;
    }
}