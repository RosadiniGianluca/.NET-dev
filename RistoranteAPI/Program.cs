using Microsoft.EntityFrameworkCore;
using RistoranteAPI.Data;
using RistoranteAPI.Services;
using RistoranteAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ValidationService>();
builder.Services.Configure<ReservationSettings>(
    builder.Configuration.GetSection("ReservationSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EmailService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
