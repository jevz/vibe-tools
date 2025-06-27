using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using VibeTools.Api.Data;
using VibeTools.Api.Interfaces;
using VibeTools.Api.Models;
using VibeTools.Api.Routes;

namespace VibeTools.Api.Repositories;

public class ToolRepository(VibeToolsContext db, IDistributedCache cache, ILogger<ToolRepository> logger) : IToolRepository
{
    public async Task<IEnumerable<object>> GetToolsAsync(string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            var cached = await cache.GetStringAsync("tools-list");
            if (cached != null)
            {
                logger.LogInformation("Retrieved tools from cache");
                return JsonSerializer.Deserialize<IEnumerable<object>>(cached)!;
            }
        }

        var toolQuery = db.Tools.Include(t => t.Reviews)
            .Select(t => new
            {
                t.Id,
                t.Name,
                t.Description,
                ReviewCount = t.Reviews.Count,
                AvgRating = t.Reviews.Count == 0 ? 0 : Math.Floor(t.Reviews.Average(r => r.Rating)),
                CommunityFavourite = t.Reviews.Count >= 5 && t.Reviews
                    .OrderByDescending(r => r.CreatedAt)
                    .Take(5)
                    .All(r => r.Rating == 5),
                Hidden = t.Reviews.Count >= 5 && t.Reviews
                    .OrderByDescending(r => r.CreatedAt)
                    .Take(5)
                    .All(r => r.Rating == 1),
            })
            .Where(t => !t.Hidden);

        if (!string.IsNullOrWhiteSpace(query))
        {
            toolQuery = toolQuery.Where(t =>
                t.Name.ToLower().Contains(query.ToLower()) ||
                t.Description.ToLower().Contains(query.ToLower()));
        }

        var tools = await toolQuery.OrderByDescending(t => t.AvgRating).ToListAsync();
        
        logger.LogInformation("Retrieved tools from db");
        if (!string.IsNullOrWhiteSpace(query)) return tools;
        
        logger.LogInformation("Caching fetched tools");
        
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var toolJson = JsonSerializer.Serialize(tools, options);
        await cache.SetStringAsync("tools-list", toolJson, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
        });

        return tools;
    }

    public async Task<object?> GetToolByIdAsync(int id)
    {
        var tool = await db.Tools
            .Include(t => t.Reviews)
            .FirstOrDefaultAsync(t => t.Id == id);
        
        if (tool == null)
        {
            logger.LogError("Tool for id {id} not found. Aborting review creation.", id);
            return null;
        }

        return new
        {
            tool.Id,
            tool.Name,
            tool.Description,
            tool.CreatedAt,
            Reviews = tool.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new { r.Id, r.Rating, r.Comment, r.CreatedAt })
        };
    }

    public async Task<Tool> AddToolAsync(Tool tool)
    {
        tool.CreatedAt = DateTime.UtcNow;
        db.Tools.Add(tool);
        await db.SaveChangesAsync();
        
        logger.LogError("Removing cached tool list - new tool added.");
        await cache.RemoveAsync("tools-list");
        return tool;
    }
}