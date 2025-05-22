using ACBackendAPI.Application.Dtos;
using FluentValidation;

namespace ACBackendAPI.Application.Validators.AdminValidatos
{
    public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentDtoValidator()
        {
            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .Length(2, 50).WithMessage("Surname must be between 2 and 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Gender must be a valid value.");

            RuleFor(x => x.Dob)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.Nationality)
                .NotEmpty().WithMessage("Nationality is required.")
                .Length(2, 50).WithMessage("Nationality must be between 2 and 50 characters.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .Length(1, 250).WithMessage("Address must be between 1 and 250 characters.");

            //RuleFor(x => x.PhoneNumber)
            //    .NotEmpty().WithMessage("Phone number is required.")
            //    .Matches(@"^\d{10}$").WithMessage("Phone number must be a 10-digit number.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            //RuleFor(x => x.Guardian)
            //    .NotNull().WithMessage("Guardian details are required.")
            //    .SetValidator(new Validators.GuardianDtoValidator());

            RuleFor(x => x.AcademicInformation)
                .NotNull().WithMessage("Academic Information is required.")
                .SetValidator(new AcademicInformationDtoValidator());
        }
    }
}
