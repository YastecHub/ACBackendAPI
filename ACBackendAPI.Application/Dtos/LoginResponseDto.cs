using System.Text.Json.Serialization;

namespace ACBackendAPI.Application.Dtos
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Avatar { get; set; }
    }
}
