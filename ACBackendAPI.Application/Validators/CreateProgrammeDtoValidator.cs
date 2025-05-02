using FluentValidation;
using ACBackendAPI.Application.Dtos;

namespace ACBackendAPI.Application.Validators
{
    public class CreateProgrammeDtoValidator : AbstractValidator<CreateProgrammeDto>
    {
        public CreateProgrammeDtoValidator()
        {
            RuleFor(x => x.ProgrammeName).NotEmpty();
            RuleFor(x => x.ProgrammeFee).GreaterThan(0);
        }
    }
}
