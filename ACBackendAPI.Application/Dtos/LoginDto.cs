using System.Text.Json.Serialization;

namespace ACBackendAPI.Application.Dtos
{
    public class LoginDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
