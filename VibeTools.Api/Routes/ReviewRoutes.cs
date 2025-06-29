using FluentValidation;
using VibeTools.Api.Helpers;
using VibeTools.Api.Interfaces;
using VibeTools.Api.Models;

namespace VibeTools.Api.Routes;

public static class ReviewRoutes
{
    public static void MapReviewRoutes(this WebApplication app)
    {
        app.MapPost("/tools/{id}/reviews", async (int id, Review review, IReviewRepository repo, IValidator<Review> validator) =>
        {
            var validationResult = await validator.ValidateAsync(review);
            if (!validationResult.IsValid)
            {
                var errors = ValidationHelper.ConvertValidationResultToCamelCaseDict(validationResult);
                return Results.BadRequest(errors);
            }
            
            var reviewResult = await repo.CreateReviewForToolAsync(id, review);
            return reviewResult == null ? Results.NotFound() : Results.Ok(reviewResult);
        });
    }
}