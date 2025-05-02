using FluentValidation;
using ACBackendAPI.Application.Dtos;

namespace ACBackendAPI.Application.Validators
{
    public class UpdateProgrammeDtoValidator : AbstractValidator<UpdateProgrammeDto>
    {
        public UpdateProgrammeDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.ProgrammeName).NotEmpty();
            RuleFor(x => x.ProgrammeFee).GreaterThan(0);
        }
    }
}
