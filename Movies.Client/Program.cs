﻿using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Movies.Client.ApiServices;
using Movies.Client.HttpHandlers;
var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddDbContext<MoviesClientContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("MoviesClientContext") ?? throw new InvalidOperationException("Connection string 'MoviesClientContext' not found.")));

// Add services to the container.
builder.Services.AddScoped<IMovieApiService, MovieApiService>();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options => 
    {
        options.Authority = "https://localhost:5005";
        options.ClientId = "movies_mvc_client";
        options.ClientSecret = "secret";
        options.ResponseType = "code id_token";
     // options.Scope.Add("openid");
     // options.Scope.Add("profile");
        options.Scope.Add("address");
        options.Scope.Add("email");
        options.Scope.Add("movieAPI");
        options.Scope.Add("roles");
        options.ClaimActions.MapUniqueJsonKey("role", "role");
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = JwtClaimTypes.GivenName,
            RoleClaimType = JwtClaimTypes.Role
        };

    });


//1. Create an HttpClient used for accessing the movies.API
builder.Services.AddTransient<AuthenticationDelegatingHandler>();
builder.Services.AddHttpClient("MovieAPIClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5010/");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
})
    .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

//2. Create an HttpClient used for accessing the IDP
builder.Services.AddHttpClient("IDPclient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5005/");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

  builder.Services.AddHttpContextAccessor();


//builder.Services.AddSingleton(new ClientCredentialsTokenRequest
//{
//    Address = "https://localhost:5005/connect/token",
//    ClientId = "movieClient",
//    ClientSecret = "secret",
//    Scope = "movieAPI"
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
