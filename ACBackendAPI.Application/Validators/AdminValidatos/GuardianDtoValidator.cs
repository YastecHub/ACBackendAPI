using ACBackendAPI.Application.Dtos;
using FluentValidation;

namespace ACBackendAPI.Application.Validators.AdminValidatos
{
    public class GuardianDtoValidator : AbstractValidator<GuardianDto>
    {
        public GuardianDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Guardian's first name is required.")
                .Length(2, 50).WithMessage("Guardian's first name must be between 2 and 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Guardian's last name is required.")
                .Length(2, 50).WithMessage("Guardian's last name must be between 2 and 50 characters.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Guardian's surname is required.")
                .Length(2, 50).WithMessage("Guardian's surname must be between 2 and 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Guardian's email is required.")
                .EmailAddress().WithMessage("Guardian's email must be a valid email address.");

            RuleFor(x => x.RelationShip)
                .NotEmpty().WithMessage("Guardian's relationship with the student is required.")
                .Length(3, 50).WithMessage("Relationship must be between 3 and 50 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Guardian's phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Guardian's phone number must be a 10-digit number.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Guardian's address is required.")
                .Length(10, 250).WithMessage("Guardian's address must be between 10 and 250 characters.");
        }
    }
}
