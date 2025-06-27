using VibeTools.Api.Interfaces;
using VibeTools.Api.Models;

namespace VibeTools.Api.Routes;

public static class ToolRoutes
{
    public static void MapToolRoutes(this WebApplication app)
    {
        app.MapGet("/tools", async (string? query, IToolRepository repo) =>
        {
            var tools = await repo.GetToolsAsync(query);
            return Results.Ok(tools);
        });

        app.MapGet("/tools/{id}", async (int id, IToolRepository repo) =>
        {
            var tool = await repo.GetToolByIdAsync(id);
            return tool is not null ? Results.Ok(tool) : Results.NotFound();
        });

        app.MapPost("/tools", async (Tool tool, IToolRepository repo) =>
        {
            var created = await repo.AddToolAsync(tool);
            return Results.Created($"/tools/{created.Id}", created);
        });
    }
}