using System.Text.Json.Serialization;

namespace DDDPractice.Application.Shared;

public class PagedResponse<T>
{
    [JsonPropertyName("products")]
    public IEnumerable<T> Data { get; set; } = [];

    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; set; } = new();
}

public class Pagination
{
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("itemsPerPage")]
    public int ItemsPerPage { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}