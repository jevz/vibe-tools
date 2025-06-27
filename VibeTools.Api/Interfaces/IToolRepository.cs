using VibeTools.Api.Models;

namespace VibeTools.Api.Interfaces;

public interface IToolRepository
{
    Task<IEnumerable<object>> GetToolsAsync(string? query);
    Task<object?> GetToolByIdAsync(int id);
    Task<Tool> AddToolAsync(Tool tool);
}
