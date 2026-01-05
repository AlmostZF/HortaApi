namespace HortaGestao.Application.DTOs.Response;

public class AuthResponseDto
{
        public string BearerToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }

}