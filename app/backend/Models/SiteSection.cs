namespace CashewCms.Api.Models;

public class SiteSection
{
    public int Id { get; set; }
    public string Key { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }
    public int SortOrder { get; set; }
    public string Status { get; set; } = "published";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public List<SiteItem> Items { get; set; } = new();
}
