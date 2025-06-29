using FluentValidation;
using VibeTools.Api.Models;

namespace VibeTools.Api.Validators;

public class ReviewValidator : AbstractValidator<Review>
{
    public ReviewValidator()
    {
        RuleFor(r => r.ToolId)
            .NotEmpty().WithMessage("ToolId must be a valid ID.");

        RuleFor(r => r.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(r => r.Comment)
            .NotEmpty().WithMessage("Comment is required.")
            .MaximumLength(1000).WithMessage("Comment can't exceed 1000 characters.");
    }
}