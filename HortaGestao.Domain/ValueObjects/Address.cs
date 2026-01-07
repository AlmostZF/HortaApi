namespace HortaGestao.Domain.ValueObjects;

public record Address(
        string Street,
        string Number,
        string City,
        string ZipCode,
        string State,
        string Neighborhood
    );