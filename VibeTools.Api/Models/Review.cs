namespace VibeTools.Api.Models;

public class Review
{
    public int Id { get; set; }
    public int ToolId { get; set; }
    public int Rating { get; set; } // 1-5
    public string Comment { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}