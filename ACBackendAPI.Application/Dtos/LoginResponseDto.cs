using System.Text.Json.Serialization;

namespace ACBackendAPI.Application.Dtos
{
    public class LoginResponseDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }
        public string Token { get; set; }
    }
}
