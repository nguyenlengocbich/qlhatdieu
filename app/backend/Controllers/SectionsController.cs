using CashewCms.Api.Data;
using CashewCms.Api.DTOs;
using CashewCms.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashewCms.Api.Controllers;

[ApiController]
[Route("api/sections")]
public class SectionsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await db.SiteSections.OrderBy(x => x.SortOrder).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var section = await db.SiteSections.FindAsync(id);
        return section is null ? NotFound(new { error = "Không tìm thấy section" }) : Ok(section);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateSectionRequest request)
    {
        var section = await db.SiteSections.FindAsync(id);
        if (section is null) return NotFound(new { error = "Không tìm thấy section" });

        section.Name = request.Name ?? "";
        section.Title = request.Title;
        section.Subtitle = request.Subtitle;
        section.Description = request.Description;
        section.Content = request.Content;
        section.Image = request.Image;
        section.Status = string.IsNullOrWhiteSpace(request.Status) ? "published" : request.Status;
        section.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(section);
    }

    [HttpGet("{id:int}/items")]
    public async Task<IActionResult> GetItems(int id)
    {
        var section = await db.SiteSections.FindAsync(id);
        if (section is null) return NotFound(new { error = "Không tìm thấy section" });
        return Ok(await db.SiteItems.Where(x => x.SectionKey == section.Key).OrderBy(x => x.SortOrder).ToListAsync());
    }

    [Authorize]
    [HttpPost("{id:int}/items")]
    public async Task<IActionResult> AddItem(int id, CreateItemRequest request)
    {
        var section = await db.SiteSections.FindAsync(id);
        if (section is null) return NotFound(new { error = "Không tìm thấy section" });
        if (string.IsNullOrWhiteSpace(request.Title)) return BadRequest(new { error = "Tiêu đề item không được để trống" });

        var max = await db.SiteItems.Where(x => x.SectionKey == section.Key).Select(x => (int?)x.SortOrder).MaxAsync() ?? 0;
        var item = new SiteItem
        {
            SectionKey = section.Key,
            Title = request.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            Icon = request.Icon,
            Image = request.Image,
            SortOrder = max + 1,
            Status = string.IsNullOrWhiteSpace(request.Status) ? "published" : request.Status
        };
        db.SiteItems.Add(item);
        await db.SaveChangesAsync();
        return Created($"/api/items/{item.Id}", item);
    }
}
