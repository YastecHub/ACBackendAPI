﻿using ACBackendAPI.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace ACBackendAPI.Application.Dtos
{
    public class StudentRegistrationDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("avatar")]
        public IFormFile Avatar { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

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
        public GuardianRegistrationDto GuardianInformation { get; set; }


        [JsonPropertyName("academicInformation")]
        public AcademicInformationRegistrationDto AcademicInformation { get; set; }
    }
}
