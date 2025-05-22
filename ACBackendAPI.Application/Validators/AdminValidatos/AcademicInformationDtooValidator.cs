using ACBackendAPI.Application.Dtos;
using FluentValidation;

namespace ACBackendAPI.Application.Validators.AdminValidatos
{
    public class AcademicInformationDtoValidator : AbstractValidator<AcademicInformationDto>
    {
        public AcademicInformationDtoValidator()
        {
            RuleFor(x => x.Department)
                .IsInEnum().WithMessage("Department must be a valid department.");

            RuleFor(x => x.CourseOfStudy)
                .NotEmpty().WithMessage("Course of study is required.")
                .Length(2, 100).WithMessage("Course of study must be between 2 and 100 characters.");

            RuleFor(x => x.ProgrammeId)
                .NotEmpty().WithMessage("Programme ID is required.");
        }
    }
}
