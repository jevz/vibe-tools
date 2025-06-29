using Microsoft.Extensions.Caching.Distributed;
using VibeTools.Api.Data;
using VibeTools.Api.Interfaces;
using VibeTools.Api.Models;

namespace VibeTools.Api.Repositories;

public class ReviewRepository(VibeToolsContext db, IDistributedCache cache, ILogger<ReviewRepository> logger) : IReviewRepository
{
    public async Task<Review?> CreateReviewForToolAsync(Review review)
    {
        var tool = await db.Tools.FindAsync(review.ToolId);
        if (tool == null)
        {
            logger.LogError("Could not find tool with id {id}. Aborting review creation", review.ToolId);
            return null;
        }

        review.CreatedAt = DateTime.UtcNow;

        db.Reviews.Add(review);
        await db.SaveChangesAsync();
        
        logger.LogInformation("Removing cached tool list - new tool review added.");
        await cache.RemoveAsync("tools-list");

        return review;
    }
}