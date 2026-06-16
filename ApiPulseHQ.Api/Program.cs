using ApiPulseHQ.Api.Services.Auth;
using ApiPulseHQ.Api.Services.Email;
using ApiPulseHQ.Api.Services.Monitoring;
using ApiPulseHQ.Api.Services.PublicStatusPage;
using ApiPulseHQ.Api.Services.ServiceEndpoints;
using ApiPulseHQ.Api.Services.StatusPages;
using ApiPulseHQ.Api.Workers;
using ApiPulseHQ.Application.Interfaces;
using ApiPulseHQ.Infrastructure.Persistence;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------
// Controllers + Swagger
// --------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --------------------------------------
// CORS (Fix for Angular + Swagger)
// --------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// --------------------------------------
// DbContext
// --------------------------------------
builder.Services.AddDbContext<ApiPulseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --------------------------------------
// JWT Authentication
// --------------------------------------
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// --------------------------------------
// Dependency Injection
// --------------------------------------
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IServiceEndpointsService, ServiceEndpointsService>();
builder.Services.AddScoped<IStatusPagesService, StatusPagesService>();
builder.Services.AddScoped<IPublicStatusPageService, PublicStatusPageService>();
builder.Services.AddScoped<IMonitoringService, MonitoringService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddHttpClient();
builder.Services.AddHostedService<MonitoringWorker>();

// --------------------------------------
// Build App
// --------------------------------------
var app = builder.Build();

// --------------------------------------
// Middleware Pipeline
// --------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPulseHQ v1");
        c.RoutePrefix = "swagger";
    });
}

// IMPORTANT: Enable CORS BEFORE auth
app.UseCors("AllowAngular");

// IMPORTANT: Disable HTTPS redirection (Swagger breaks otherwise)
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
