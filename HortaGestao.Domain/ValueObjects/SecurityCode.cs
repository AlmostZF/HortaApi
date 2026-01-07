namespace HortaGestao.Domain.ValueObjects;

public class SecurityCode
{
    public string Value { get; set; }

    protected SecurityCode() { }
    public SecurityCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Invalid code.");

        Value = value;
    }

    public string GenerateSecurityCode(int length = 4)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();

        return new string(
            Enumerable.Range(0, length)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray()
        );
    }
    
    public override bool Equals(object? obj)
    {
        return obj is SecurityCode other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}