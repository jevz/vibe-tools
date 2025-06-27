using VibeTools.Api.Interfaces;
using VibeTools.Api.Models;
using VibeTools.Api.Repositories;

namespace VibeTools.Api.Routes;

public static class ReviewRoutes
{
    public static void MapReviewRoutes(this WebApplication app)
    {
        app.MapPost("/tools/{id}/reviews", async (int id, Review review, IReviewRepository repo) =>
        {
            var reviewResult = await repo.CreateReviewForToolAsync(id, review);
            return reviewResult == null ? Results.NotFound() : Results.Ok(reviewResult);
        });
    }
}