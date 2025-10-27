namespace DDD_Practice.DDDPractice.Domain.ValueObjects;

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

    public override bool Equals(object? obj)
    {
        return obj is SecurityCode other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}