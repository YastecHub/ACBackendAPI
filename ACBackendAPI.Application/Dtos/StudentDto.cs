using ACBackendAPI.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace ACBackendAPI.Application.Dtos
{
    public class StudentDto
    {
        public string? Avatar { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Gender Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public GuardianDto Guardian { get; set; }
        public AcademicInformationDto AcademicInformation { get; set; }
    }

    public class GuardianDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string RelationShip { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class AcademicInformationDto
    {
        public Department Department { get; set; }
        public string CourseOfStudy { get; set; }
        public Guid ProgrammeId { get; set; }
    }

    public class CreateStudentDto
    {
        public IFormFile? Avatar { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Gender Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public GuardianDto Guardian { get; set; }
        public AcademicInformationDto AcademicInformation { get; set; }
    }
}
