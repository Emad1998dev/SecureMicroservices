using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Ocelot.json");
var authenticationProviderKey = "IdentityApiKey";
builder.Services.AddAuthentication()
    .AddJwtBearer(authenticationProviderKey, x =>
    {
        x.Authority = "https://localhost:5005";
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };

    });
builder.Services.AddOcelot();

var app = builder.Build();


await app.UseOcelot();

app.MapControllers();

app.Run();


