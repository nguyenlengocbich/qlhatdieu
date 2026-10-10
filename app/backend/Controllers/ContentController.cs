using CashewCms.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashewCms.Api.Controllers;

[ApiController]
[Route("api/content")]
public class ContentController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var sections = await db.SiteSections
            .Where(x => x.Status == "published")
            .OrderBy(x => x.SortOrder)
            .ToListAsync();

        var keys = sections.Select(x => x.Key).ToList();
        var items = await db.SiteItems
            .Where(x => keys.Contains(x.SectionKey) && x.Status == "published")
            .OrderBy(x => x.SortOrder)
            .ToListAsync();

        var resultSections = sections.Select(s => new
        {
            id = s.Id,
            key = s.Key,
            name = s.Name,
            title = s.Title,
            subtitle = s.Subtitle,
            description = s.Description,
            content = s.Content,
            image = s.Image,
            status = s.Status,
            updatedAt = s.UpdatedAt,
            items = items.Where(i => i.SectionKey == s.Key).ToList()
        }).ToList();

        var hero = resultSections.FirstOrDefault(x => x.key == "hero");
        var cta = resultSections.FirstOrDefault(x => x.key == "cta");

        return Ok(new
        {
            hero = new
            {
                badge = hero?.subtitle ?? "Phần mềm quản lý sản xuất & kinh doanh hạt điều",
                title = hero?.title ?? "Quản lý toàn diện hoạt động hạt điều",
                description = hero?.description ?? "",
                primaryButton = "Khám phá tính năng",
                secondaryButton = "Xem quy trình"
            },
            sections = resultSections,
            cta = new
            {
                badge = cta?.subtitle ?? "Giải pháp quản lý toàn diện",
                title = cta?.title ?? "Quản lý toàn bộ quy trình trên một nền tảng",
                description = cta?.description ?? "",
                status = cta?.status ?? "published",
                updatedAt = cta?.updatedAt ?? DateTime.UtcNow
            }
        });
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateContentRequest request)
    {
        if (request.Hero is not null)
        {
            var hero = await db.SiteSections.SingleOrDefaultAsync(x => x.Key == "hero");
            if (hero is null) return NotFound(new { message = "Không tìm thấy section hero." });

            hero.Subtitle = request.Hero.Badge;
            hero.Title = request.Hero.Title;
            hero.Description = request.Hero.Description;
            hero.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }

        var response = await BuildContentResponse();
        return Ok(response);
    }

    private async Task<object> BuildContentResponse()
    {
        var sections = await db.SiteSections.Where(x => x.Status == "published").OrderBy(x => x.SortOrder).ToListAsync();
        var keys = sections.Select(x => x.Key).ToList();
        var items = await db.SiteItems.Where(x => keys.Contains(x.SectionKey) && x.Status == "published").OrderBy(x => x.SortOrder).ToListAsync();
        var hero = sections.FirstOrDefault(x => x.Key == "hero");
        var cta = sections.FirstOrDefault(x => x.Key == "cta");
        var mapped = sections.Select(s => new { id=s.Id, key=s.Key, name=s.Name, title=s.Title, subtitle=s.Subtitle, description=s.Description, content=s.Content, image=s.Image, status=s.Status, updatedAt=s.UpdatedAt, items=items.Where(i=>i.SectionKey==s.Key).ToList() }).ToList();
        return new
        {
            hero = new { badge=hero?.Subtitle ?? "Phần mềm quản lý sản xuất & kinh doanh hạt điều", title=hero?.Title ?? "Quản lý toàn diện hoạt động hạt điều", description=hero?.Description ?? "", primaryButton="Khám phá tính năng", secondaryButton="Xem quy trình" },
            sections=mapped,
            cta=new { badge=cta?.Subtitle ?? "Giải pháp quản lý toàn diện", title=cta?.Title ?? "Quản lý toàn bộ quy trình trên một nền tảng", description=cta?.Description ?? "", status=cta?.Status ?? "published", updatedAt=cta?.UpdatedAt ?? DateTime.UtcNow }
        };
    }

    public sealed record UpdateContentRequest(HeroUpdate? Hero);
    public sealed record HeroUpdate(string Badge, string Title, string Description, string PrimaryButton, string SecondaryButton);
}
