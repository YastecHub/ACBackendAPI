using FluentValidation;
using ACBackendAPI.Application.Dtos;

namespace ACBackendAPI.Application.Validators
{
    public class AcademicInformationDtoValidator : AbstractValidator<AcademicInformationRegistrationDto>
    {
        public AcademicInformationDtoValidator()
        {
            RuleFor(x => x.CourseOfStudy)
                .NotEmpty().WithMessage("Course of study is required.");

            RuleFor(x => x.ProgrammeId)
                .NotEmpty().WithMessage("Programme is required.");
        }
    }
}
