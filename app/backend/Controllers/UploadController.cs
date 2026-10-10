
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashewCms.Api.Controllers;

[ApiController]
[Route("api/upload")]
public class UploadController(IConfiguration configuration) : ControllerBase
{
    [Authorize]
    [HttpPost]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "Vui lòng chọn hình ảnh" });

        var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowed.Contains(ext))
            return BadRequest(new { error = "Chỉ hỗ trợ JPG, PNG, WEBP hoặc GIF" });

        if (file.Length > 8 * 1024 * 1024)
            return BadRequest(new { error = "Kích thước tối đa 8MB" });

        var cloudName = configuration["Cloudinary:CloudName"];
        var apiKey = configuration["Cloudinary:ApiKey"];
        var apiSecret = configuration["Cloudinary:ApiSecret"];

        if (string.IsNullOrWhiteSpace(cloudName) ||
            string.IsNullOrWhiteSpace(apiKey) ||
            string.IsNullOrWhiteSpace(apiSecret))
        {
            return StatusCode(500, new { error = "Cloudinary chưa được cấu hình" });
        }

        var account = new Account(cloudName, apiKey, apiSecret);
        var cloudinary = new Cloudinary(account);

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "cashew-cms",
            PublicId = Guid.NewGuid().ToString("N"),
            Overwrite = false
        };

        var result = await cloudinary.UploadAsync(uploadParams);

        if (result.Error != null || result.SecureUrl == null)
        {
            return StatusCode(500, new
            {
                error = result.Error?.Message ?? "Upload ảnh thất bại"
            });
        }
        return Ok(new { url = result.SecureUrl.ToString() });
    }
}
