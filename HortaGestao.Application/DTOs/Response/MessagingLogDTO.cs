using HortaGestao.Application.DTOs.Request;

namespace HortaGestao.Application.DTOs.Response;

public class MessagingLogDto
{
    public string MessageError { get; set; }
    public DateTime CreatedAt { get; set; }
    public ProductCreateDto ProductDto { get; set; }
    public string LineError { get; set; }
    
}