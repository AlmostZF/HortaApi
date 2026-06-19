namespace HortaGestao.Application.DTOs.Response;

public class MessagingLogDto
{
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; }
    public string LineError { get; set; }
    public bool IsSuccess { get; set; }
    public int SuccessCount { get; set; }
    public int ErrorCount { get; set; }
    public List<string> Errors { get; set; } = new();
    
    
}