using FluentValidation;
using ACBackendAPI.Application.Dtos;

namespace ACBackendAPI.Application.Validators
{
    public class StudentDtoValidator : AbstractValidator<StudentDto>
    {
        public StudentDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Phone number must be between 10 and 15 digits and contain only numbers.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.");

            RuleFor(x => x.Dob)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.Nationality)
                .NotEmpty().WithMessage("Nationality is required.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(x => x.GuardianInformation)
                .SetValidator(new GuardianDtoValidator());

            RuleFor(x => x.AcademicInformation)
                .SetValidator(new AcademicInformationDtoValidator());

        }
    }
}
