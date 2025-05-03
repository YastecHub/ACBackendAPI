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
                .NotEmpty()
                .WithMessage("Guardian phone number is required.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Guardian address is required.");
        }
    }
}
