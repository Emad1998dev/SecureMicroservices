using Duende.IdentityServer.Test;
using IdentityServer;
using IdentityServerHost;

var builder = WebApplication.CreateBuilder(args);

// اضافه کردن IdentityServer
builder.Services.AddIdentityServer(options =>
{
    options.EmitStaticAudienceClaim = true; // تنظیم ضروری برای OAuth
})
.AddInMemoryClients(Config.Clients) // کلاینت‌ها
.AddInMemoryIdentityResources(Config.IdentityResources) // منابع هویتی
.AddInMemoryApiScopes(Config.ApiScopes) // اسکوپ‌های API
.AddTestUsers(TestUsers.Users); // کاربران تستی

// اضافه کردن Razor Pages برای UI
builder.Services.AddRazorPages();

var app = builder.Build();

// تنظیم middlewareها
app.UseStaticFiles(); // استفاده از فایل‌های استاتیک
app.UseRouting(); // فعال کردن مسیر‌دهی
app.UseIdentityServer(); // اضافه کردن IdentityServer به pipeline
app.UseAuthorization(); // فعال کردن Authorization

// تنظیم مسیرها
app.MapRazorPages();

app.Run();
