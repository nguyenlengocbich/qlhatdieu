using CashewCms.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CashewCms.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!await db.AdminUsers.AnyAsync())
        {
            // Hash được lấy nguyên từ AdminUser trong dev.db hiện tại.
            db.AdminUsers.Add(new AdminUser
            {
                Email = "admin@example.com",
                PasswordHash = "$2b$12$UDthyqBBET4PqSc/X.igEeKHD792s7HoRxtC70EI8MVQUkfsI1D8G",
                Name = "Administrator",
                Status = "active"
            });
            await db.SaveChangesAsync();
        }

        if (!await db.SiteSections.AnyAsync())
        {
            db.SiteSections.AddRange(
                new SiteSection { Key="hero", Name="Hero / Giới thiệu", Title="Quản lý toàn diện hoạt động hạt điều", Subtitle="Phần mềm quản lý sản xuất & kinh doanh hạt điều", Description="Quản lý xuyên suốt từ mua nguyên liệu, nhập kho, sản xuất chế biến, quản lý thành phẩm đến bán hàng, công nợ và báo cáo hiệu quả kinh doanh.", Content="", Image="", SortOrder=1, Status="published" },
                new SiteSection { Key="features", Name="Tính năng", Title="Các tính năng chính", Subtitle="", Description="Quản lý toàn diện các nghiệp vụ sản xuất và kinh doanh.", Content="", Image="", SortOrder=2, Status="published" },
                new SiteSection { Key="process", Name="Quy trình", Title="Quy trình vận hành", Description="Theo dõi toàn bộ quy trình từ mua hàng đến bán hàng và báo cáo.", SortOrder=3, Status="published" },
                new SiteSection { Key="warehouses", Name="Hệ thống kho", Title="Quản lý hệ thống kho", Subtitle="", Description="Quản lý kho nguyên liệu, kho chế biến và kho thành phẩm.", Content="", Image="", SortOrder=4, Status="published" },
                new SiteSection { Key="production", Name="Sản xuất", Title="Quản lý sản xuất", Subtitle="", Description="Theo dõi lô sản xuất, chế biến và giá vốn.", Content="", Image="/uploads/1790779507038-51774506-7f16-4ce6-9369-1c11f7234467.png", SortOrder=5, Status="published" },
                new SiteSection { Key="internal-transfer", Name="Luân chuyển nội bộ", Title="Điều chuyển giữa các kho", Subtitle="", Description="Quản lý luân chuyển hàng hóa và giao dịch nội bộ.", Content="", Image="/uploads/1790775981012-8520e36d-74e3-45ef-82df-01d7612d0d42.png", SortOrder=6, Status="published" },
                new SiteSection { Key="reports", Name="Báo cáo", Title="Báo cáo và phân tích", Description="Theo dõi tồn kho, doanh thu, chi phí và lãi/lỗ.", SortOrder=7, Status="published" },
                new SiteSection { Key="screenshots", Name="Giao diện hệ thống", Title="Giao diện phần mềm", Description="Hình ảnh minh họa các màn hình nghiệp vụ.", SortOrder=8, Status="published" },
                new SiteSection { Key="cta", Name="CTA / Liên hệ", Title="Quản lý toàn bộ quy trình trên một nền tảng", Description="Từ mua nguyên liệu, sản xuất chế biến, quản lý kho đến bán hàng và báo cáo.", SortOrder=9, Status="published" }
            );
            await db.SaveChangesAsync();

            AddItems(db, "features", new[]
            {
                ("Quản lý mua hàng", "Quản lý đơn mua, nhập hàng và công nợ nhà cung cấp.", "ShoppingCart", (string?)null),
                ("Quản lý sản xuất", "Quản lý lô sản xuất, chế biến và thành phẩm.", (string?)null, (string?)null),
                ("Quản lý kho", "Theo dõi tồn kho và luân chuyển hàng hóa.", (string?)null, (string?)null),
                ("Quản lý bán hàng", "Quản lý đơn bán, giao hàng và công nợ khách hàng.", (string?)null, (string?)null),
                ("Luân chuyển nội bộ", "Điều chuyển hàng hóa giữa các kho.", (string?)null, (string?)null),
                ("Báo cáo", "Theo dõi doanh thu, chi phí, tồn kho và lãi/lỗ.", (string?)null, (string?)null)
            });

            AddItems(db, "process", new[]
            {
                ("Mua nguyên liệu", "Tạo đơn mua và quản lý công nợ nhà cung cấp.", (string?)null, (string?)null),
                ("Nhập kho", "Ghi nhận hàng thực tế nhập vào hệ thống.", (string?)null, (string?)null),
                ("Sản xuất & chế biến", "Quản lý lô và thực hiện các công đoạn sản xuất.", (string?)null, (string?)null),
                ("Quản lý thành phẩm", "Điều chuyển thành phẩm sang kho thành phẩm.", (string?)null, (string?)null),
                ("Bán hàng", "Tạo đơn bán và quản lý giao hàng.", (string?)null, (string?)null),
                ("Thu tiền", "Theo dõi thu tiền và công nợ khách hàng.", (string?)null, (string?)null),
                ("Báo cáo", "Theo dõi tồn kho và hiệu quả kinh doanh.", (string?)null, (string?)null)
            });

            AddItems(db, "warehouses", new[]
            {
                ("Kho nguyên liệu", "Quản lý nguyên liệu đầu vào, nhập kho, tồn kho và các lô nguyên liệu.", "Boxes", (string?)null),
                ("Kho chế biến", "Quản lý lô sản xuất, chế biến và quá trình chuyển đổi nguyên liệu.", "Factory", (string?)null),
                ("Kho thành phẩm", "Quản lý thành phẩm sau chế biến, nhập xuất kho và phục vụ bán hàng.", "PackageCheck", (string?)null)
            });

            AddItems(db, "production", new[]
            {
                ("Quản lý lô", "Theo dõi nguyên liệu theo từng lô và kiểm soát trạng thái lô trong quá trình sản xuất.", "CircleDot", (string?)null),
                ("Lệnh sản xuất", "Tạo lệnh sản xuất dựa trên sản phẩm và số lượng nguyên liệu trong lô.", "ClipboardList", (string?)null),
                ("Chế biến", "Theo dõi các công đoạn sản xuất và lượng nguyên liệu đầu vào, thành phẩm đầu ra.", "Factory", (string?)null),
                ("Đóng lô & chốt giá vốn", "Đóng lô và hoàn thiện giá trị tài chính sau khi quá trình sản xuất hoàn tất.", "PackageCheck", (string?)null)
            });

            AddItems(db, "internal-transfer", new[]
            {
                ("Giao dịch nội bộ", "Theo dõi các giao dịch điều chuyển hàng hóa giữa các kho trong doanh nghiệp.", "ArrowLeftRight", (string?)null),
                ("Giá trị nội bộ", "Giá trị điều chuyển được dùng làm cơ sở tính chi phí tại kho nhận.", "CircleDollarSign", (string?)null),
                ("Phân tích theo kho", "Dữ liệu điều chuyển hỗ trợ phân tích doanh thu, giá vốn và lãi/lỗ theo kho.", "BarChart3", (string?)null)
            });

            AddItems(db, "reports", new[]
            {
                ("Doanh thu, chi phí & lợi nhuận", "Theo dõi doanh thu, chi phí, giá vốn và lợi nhuận theo tháng, quý, năm hoặc khoảng thời gian tùy chọn.", "TrendingUp", "/uploads/1790775451508-6f108b15-83b9-4320-bbf6-f3fbbb4da773.png"),
                ("Tồn kho ma trận", "Theo dõi tồn đầu kỳ, nhập kho, tồn hiện tại và số lượng sản phẩm theo từng kho.", "Boxes", "/images/inventory.png"),
                ("Lãi / lỗ kho nguyên liệu", "Theo dõi doanh thu, giá vốn, chi phí và kết quả kinh doanh của kho nguyên liệu.", "Boxes", "/images/profit-raw-material.png"),
                ("Lãi / lỗ kho chế biến", "Theo dõi chi phí sản xuất, giá vốn và hiệu quả hoạt động tại kho chế biến.", "Factory", "/images/profit-processing.png"),
                ("Lãi / lỗ kho thành phẩm", "Theo dõi doanh thu, giá vốn và lợi nhuận từ thành phẩm được xuất bán.", "PackageCheck", "/images/profit-finished-goods.png")
            });

            AddItems(db, "screenshots", new[]
            {
                ("Tổng quan hệ thống", "Theo dõi nhanh tình hình hoạt động của doanh nghiệp.", (string?)null, "/images/dashboard.png"),
                ("Quản lý sản xuất", "Theo dõi lô, lệnh sản xuất và các công đoạn chế biến.", (string?)null, "/images/production.png"),
                ("Luân chuyển giữa các kho", "Quản lý quá trình điều chuyển hàng hóa nội bộ.", (string?)null, "/images/internal-transfer.png")
            });
        }

        await db.SaveChangesAsync();
    }

    private static void AddItems(AppDbContext db, string sectionKey, IEnumerable<(string title, string description, string? icon, string? image)> rows)
    {
        var sort = 1;
        foreach (var row in rows)
        {
            db.SiteItems.Add(new SiteItem
            {
                SectionKey = sectionKey,
                Title = row.title,
                Description = row.description,
                Icon = row.icon,
                Image = row.image,
                SortOrder = sort++,
                Status = "published"
            });
        }
    }
}
