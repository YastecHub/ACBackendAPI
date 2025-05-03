using FluentValidation;

namespace ACBackendAPI.Application.Validators
{
    public class GuardianDtoValidator : AbstractValidator<GuardianDto>
    {
        public GuardianDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Guardian name is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Guardian email is required.")
                .EmailAddress()
                .WithMessage("Guardian email must be valid.");

            RuleFor(x => x.Relationship)
                .NotEmpty()
                .WithMessage("Relationship is required.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Phone number must be between 10 and 15 digits and contain only numbers.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Guardian address is required.");
        }
    }
}
