using ACBackendAPI.Domain.Enum;
using System.Text.Json.Serialization;

namespace ACBackendAPI.Application.Dtos
{
    public class StudentDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("gender")]
        public Gender Gender { get; set; }

        [JsonPropertyName("dob")]
        public DateTime Dob { get; set; }

        [JsonPropertyName("nationality")]
        public string Nationality { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("guardianInformation")]
        public GuardianDto GuardianInformation { get; set; }


        [JsonPropertyName("academicInformation")]
        public AcademicInformationDto AcademicInformation { get; set; }
    }
}
