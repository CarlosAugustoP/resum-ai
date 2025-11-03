using Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resumai.Db;
using Resumai.Services.Domain;
using ZiggyCreatures.Caching.Fusion;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddAutoMapper(
    cfg => {},
    typeof(Program)
);

builder.Services.AddControllers();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ResumeService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddFusionCache().AsHybridCache();

var app = builder.Build();

app.UseMiddleware<Resumai.Middlewares.CatchExceptionMiddleware>();
app.UseMiddleware<Resumai.Middlewares.UserValidationMiddleware>();
app.MapControllers();

app.Run();
