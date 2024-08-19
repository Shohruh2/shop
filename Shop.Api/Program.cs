using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shop.Api;
using Shop.Api.Middleware;
using Shop.Application.Validators;
using Shop.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// ---- Authentication
builder.Services.AddCognitoIdentity();
// ---- Authentication end

// builder.Configuration.GetAWSOptions<>()
builder.Services.AddAuthentication(options => 
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = "https://cognito-idp.eu-north-1.amazonaws.com/eu-north-1_2sVtDUSZu";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidAudience = "t4maqhd858lfj4jtf3ib4kptv"
        };
    });
// authorization

// authorization end

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Добавьте параметры OAuth
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("https://sho-juh.auth.eu-north-1.amazoncognito.com/oauth2/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "OpenID scope" },
                    { "email", "Email scope" },
                    // Добавьте другие необходимые scopes
                }
            }
        }
    });

    // Добавьте политику безопасности для OAuth
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            new List<string>()
        }
    });
});

builder.Services.AddLogging();

builder.Services.AddDbContext<ApiDbContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsApiConnectionString")));

builder.Services.AddApplication(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseMiddleware<CustomExceptionMiddleware>();

app.MapControllers();

app.Run();