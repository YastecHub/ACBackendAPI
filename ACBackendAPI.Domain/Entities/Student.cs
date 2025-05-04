using ACBackendAPI.Domain.Entities;
using ACBackendAPI.Domain.Enum;

public class Student : BaseEntity
{
    public string? Avatar { get; set; }
    public string Surname { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public string GenderDesc { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime Dob { get; set; }
    public string Nationality { get; set; }
    public string Address { get; set; }

    public Guid GuardianId { get; set; }
    public Guardian Guardian { get; set; }

    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public AcademicInformation AcademicInformation { get; set; }

    public Student()
    {
        GenderDesc = Gender.ToString();
    }
}
