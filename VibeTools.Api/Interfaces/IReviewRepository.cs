using VibeTools.Api.Models;

namespace VibeTools.Api.Interfaces;

public interface IReviewRepository
{
    Task<Review?> CreateReviewForToolAsync(int toolId, Review review);
}
