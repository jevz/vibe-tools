using VibeTools.Api.Models;

namespace VibeTools.Api.Data;

public static class SeedData
{
    public static void Initialize(VibeToolsContext db)
    {
        if (db.Tools.Any()) return;

        var chatGpt = new Tool
        {
            Name = "ChatGPT",
            Description = "Conversational AI by OpenAI",
            CreatedAt = DateTime.UtcNow,
            Reviews = new List<Review>()
            {
                new() { Rating = 5, Comment = "Absolutely love it!", CreatedAt = DateTime.UtcNow },
                new() { Rating = 5, Comment = "Game changer", CreatedAt = DateTime.UtcNow },
                new() { Rating = 5, Comment = "Incredible responses", CreatedAt = DateTime.UtcNow },
                new() { Rating = 5, Comment = "My go-to assistant", CreatedAt = DateTime.UtcNow },
                new() { Rating = 5, Comment = "Community favorite!", CreatedAt = DateTime.UtcNow },
                new() { Rating = 4, Comment = "Very useful", CreatedAt = DateTime.UtcNow },
            }
        };

        var dalle = new Tool
        {
            Name = "DALL·E",
            Description = "Image generation model by OpenAI",
            CreatedAt = DateTime.UtcNow,
            Reviews = new List<Review>()
            {
                new() { Rating = 1, Comment = "Terrible results", CreatedAt = DateTime.UtcNow },
                new() { Rating = 1, Comment = "Poor quality", CreatedAt = DateTime.UtcNow },
                new() { Rating = 1, Comment = "Not impressed", CreatedAt = DateTime.UtcNow },
                new() { Rating = 2, Comment = "Waste of time", CreatedAt = DateTime.UtcNow },
                new() { Rating = 2, Comment = "Very disappointing", CreatedAt = DateTime.UtcNow },
            }
        };

        var copilot = new Tool
        {
            Name = "GitHub Copilot",
            Description = "AI pair programmer",
            CreatedAt = DateTime.UtcNow
        };
        
        var claud = new Tool
        {
            Name = "Claud",
            Description = "Conversational and Coding AI by Anthropic",
            CreatedAt = DateTime.UtcNow
        };

        db.Tools.AddRange(chatGpt, dalle, copilot, claud);
        db.SaveChanges();
    }
}