namespace HortaGestao.Domain.Entities;


public class RefreshTokenEntity
{
    public Guid Id { get; private set; }
    
    public string Token { get; private set;}
    public Guid UserId { get; private set; }
    public DateTime Expires { get;private set; }
    public DateTime AbsoluteExpires { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime Created { get; private set; }
    public string? ReplacedByToken { get; private set; }
    
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsAbsoluteExpired => DateTime.UtcNow >= AbsoluteExpires;
    public bool IsActive => !IsRevoked && !IsExpired && !IsAbsoluteExpired;

    public RefreshTokenEntity(
        string token, Guid userId, bool isRevoked,
        string? replacedByToken)
    {
        UserId = userId;
        AbsoluteExpires = DateTime.UtcNow.AddDays(7);
        Created = DateTime.UtcNow;
        Expires = DateTime.UtcNow.AddDays(1);
        Token = token;
        IsRevoked = isRevoked;
        ReplacedByToken = replacedByToken ?? null;
        Id = Guid.NewGuid();
    }

    public void Revoke(string reason, string replacedByToken = null)
    {
        IsRevoked = true;
        ReplacedByToken = replacedByToken;
    }
}