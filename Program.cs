using Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Resumai.Auth;
using Resumai.Db;
using Resumai.Services.Domain;
using ZiggyCreatures.Caching.Fusion;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddAutoMapper(
    cfg => {},
    typeof(Program)
);

builder.Services.AddControllers();
builder.Services.AddScoped<UserService>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);
builder.Services.AddSingleton<JwtService>(
    sp => new JwtService(sp.GetRequiredService<IOptions<JwtSettings>>())
);
builder.Services.AddScoped<ResumeService>();
builder.Services.AddSingleton<EmailService>(
    sp => new EmailService("SMPTKEY")
);//TODO
builder.Services.AddFusionCache().AsHybridCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<Resumai.Middlewares.CatchExceptionMiddleware>();
app.UseMiddleware<Resumai.Middlewares.UserValidationMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resumai API V1");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();

app.Run();
