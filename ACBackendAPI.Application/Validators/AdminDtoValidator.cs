using FluentValidation;
using ACBackendAPI.Application.Dtos;
using System;

namespace ACBackendAPI.Application.Validators
{
    public class AdminDtoValidator : AbstractValidator<AdminDto>
    {
        public AdminDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(x => x.Nationality)
                .NotEmpty().WithMessage("Nationality is required.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.");
        }
    }
}
