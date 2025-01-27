using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Movies.API.Data;

var builder = WebApplication.CreateBuilder(args);

// افزودن پایگاه داده In-Memory
builder.Services.AddDbContext<MoviesAPIContext>(options =>
    options.UseInMemoryDatabase("Movies"));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5005";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClientPolicy", policy => policy.RequireClaim("client_id", "movieClient" , "movies_mvc_client"));
});

// افزودن سرویس‌ها
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed کردن داده‌ها
SeedDatabase(app);

// تنظیمات مربوط به محیط توسعه
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// تنظیمات مربوط به درخواست‌های HTTP
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// متد SeedDatabase
void SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    // دریافت کانتکست پایگاه داده
    var moviesContext = services.GetRequiredService<MoviesAPIContext>();

    // فراخوانی متد Seed برای اضافه کردن داده‌ها
    MoviesContextSeed.SeedAsync(moviesContext);
}
