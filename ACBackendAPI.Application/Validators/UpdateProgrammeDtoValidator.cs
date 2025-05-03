using FluentValidation;
using ACBackendAPI.Application.Dtos;

namespace ACBackendAPI.Application.Validators
{
    public class UpdateProgrammeDtoValidator : AbstractValidator<UpdateProgrammeDto>
    {
        public UpdateProgrammeDtoValidator()
        {
            RuleFor(x => x.Id)
                  .NotEmpty().WithMessage("Programme ID is required.")
                  .Must(id => id != Guid.Empty).WithMessage("Programme ID cannot be an empty GUID.");

            RuleFor(x => x.ProgrammeName)
                .NotEmpty().WithMessage("Programme name is required.")
                .MaximumLength(100).WithMessage("Programme name cannot exceed 100 characters.");

            RuleFor(x => x.ProgrammeFee)
                .GreaterThan(0).WithMessage("Programme fee must be greater than zero.")
                .LessThan(10000).WithMessage("Programme fee cannot exceed 10,000.");
        }
    }
}
