using CashewCms.Api.Data;
using CashewCms.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashewCms.Api.Controllers;

[ApiController]
[Route("api/items")]
public class ItemsController(AppDbContext db) : ControllerBase
{
    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateItemRequest request)
    {
        var item = await db.SiteItems.FindAsync(id);
        if (item is null) return NotFound(new { error = "Không tìm thấy item" });

        item.Title = request.Title ?? "";
        item.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description;
        item.Icon = request.Icon;
        item.Image = request.Image;
        item.SortOrder = request.SortOrder ?? item.SortOrder;
        item.Status = string.IsNullOrWhiteSpace(request.Status) ? "published" : request.Status;
        item.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(item);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await db.SiteItems.FindAsync(id);
        if (item is null) return NotFound(new { error = "Không tìm thấy item" });
        db.SiteItems.Remove(item);
        await db.SaveChangesAsync();
        return Ok(new { success = true });
    }
}
