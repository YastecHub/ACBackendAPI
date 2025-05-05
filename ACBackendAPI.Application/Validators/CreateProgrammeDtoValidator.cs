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

            RuleFor(x => x.ProgrammeDescription)
                .NotEmpty()
                .WithMessage("Programme Description is required.");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Programme Description is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("Programme Description is required.");

            RuleFor(x => x)
                .Must(dto => dto.EndDate > dto.StartDate)
                .WithMessage("End date must be after start date.")
                .When(dto => dto.StartDate != default && dto.EndDate != default);
        }
    }
}
