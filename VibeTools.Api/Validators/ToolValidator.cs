using FluentValidation;
using VibeTools.Api.Models;

namespace VibeTools.Api.Validators;

public class ToolValidator : AbstractValidator<Tool>
{
    public ToolValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name can't exceed 50 characters.");

        RuleFor(t => t.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description can't exceed 1000 characters.");
    }
}