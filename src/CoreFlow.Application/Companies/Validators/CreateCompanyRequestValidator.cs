using CoreFlow.Application.Companies.DTOs;
using FluentValidation;

namespace CoreFlow.Application.Companies.Validators;

public class CreateCompanyRequestValidator
    : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Company name is required.")
            .MaximumLength(150);

        RuleFor(x => x.Document)
            .NotEmpty()
            .WithMessage("Document is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone is required.");
    }
}