using FluentValidation;

namespace ACBackendAPI.Application.Validators
{
    public class AcademicInformationDtoValidator : AbstractValidator<AcademicInformationDto>
    {
        public AcademicInformationDtoValidator()
        {
            RuleFor(x => x.CourseOfStudy).NotEmpty();
            RuleFor(x => x.ProgrammeId).NotEmpty();
        }
    }
}
