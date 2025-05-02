using FluentValidation;

namespace ACBackendAPI.Application.Validators
{
    public class GuardianDtoValidator : AbstractValidator<GuardianDto>
    {
        public GuardianDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Relationship).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
        }
    }
}
