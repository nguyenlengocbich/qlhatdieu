namespace CashewCms.Api.Models;

public class SiteItem
{
    public int Id { get; set; }
    public string SectionKey { get; set; } = "";
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Image { get; set; }
    public int SortOrder { get; set; }
    public string Status { get; set; } = "published";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
