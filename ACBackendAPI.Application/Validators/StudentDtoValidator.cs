using FluentValidation;
using ACBackendAPI.Application.Dtos;

namespace ACBackendAPI.Application.Validators
{
    public class StudentDtoValidator : AbstractValidator<StudentDto>
    {
        public StudentDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Gender).NotEmpty();
            RuleFor(x => x.Dob).NotEmpty().LessThan(DateTime.Today);
            RuleFor(x => x.Nationality).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.GuardianInformation).SetValidator(new GuardianDtoValidator());
            RuleFor(x => x.AcademicInformation).SetValidator(new AcademicInformationDtoValidator());
        }
    }
}
