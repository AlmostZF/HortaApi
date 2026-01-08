namespace HortaGestao.Domain.ValueObjects;

public class Address
{
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string City { get; private set; }
    public string ZipCode { get; private set; }
    public string State { get; private set; }
    public string Neighborhood { get; private set; }
    
    public Address(string street, string number, string city, string zipCode, string state, string neighborhood)
    {
        Street = street;
        Number = number;
        City = city;
        ZipCode = zipCode;
        State = state;
        Neighborhood = neighborhood;
    }

    private Address() { }
}