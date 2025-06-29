using VibeTools.Api.Models;

namespace VibeTools.Api.Interfaces;

public interface IReviewRepository
{
    Task<Review?> CreateReviewForToolAsync(Review review);
}
