using FluentValidation;
using ACBackendAPI.Application.Dtos;

namespace ACBackendAPI.Application.Validators
{
    public class CreateProgrammeDtoValidator : AbstractValidator<CreateProgrammeDto>
    {
        public CreateProgrammeDtoValidator()
        {
            RuleFor(x => x.ProgrammeName)
                .NotEmpty()
                .WithMessage("Programme name is required.");

            RuleFor(x => x.ProgrammeFee)
                .GreaterThan(0)
                .WithMessage("Programme fee must be greater than 0.");
        }
    }
}
