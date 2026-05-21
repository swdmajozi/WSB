using JobApplicationAPI.Models.Dtos;
using FluentValidation;

namespace JobApplicationAPI.Validators;

public class CreateApplicationValidator : AbstractValidator<CreateApplicationRequest>
{
    public CreateApplicationValidator()
    {
        RuleFor(x => x.JobId)
            .GreaterThan(0)
            .WithMessage("JobId must be greater than 0.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.")
            .MinimumLength(2)
            .WithMessage("Full name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Full name cannot exceed 200 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.")
            .MaximumLength(255)
            .WithMessage("Email cannot exceed 255 characters.");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("Phone cannot exceed 20 characters.")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.CoverLetter)
            .MaximumLength(5000)
            .WithMessage("Cover letter cannot exceed 5000 characters.")
            .When(x => !string.IsNullOrEmpty(x.CoverLetter));
    }
}
