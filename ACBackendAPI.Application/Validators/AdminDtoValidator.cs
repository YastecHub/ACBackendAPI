using FluentValidation;
using ACBackendAPI.Application.Dtos;
using System;

namespace ACBackendAPI.Application.Validators
{
    public class AdminDtoValidator : AbstractValidator<AdminDto>
    {
        public AdminDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Gender).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.Nationality).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
        }
    }
}
