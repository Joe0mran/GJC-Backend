using Microsoft.EntityFrameworkCore;
using GJC.Models;              // adjust to your actual namespace
using GJC.Data;                // where your DbContext lives

var builder = WebApplication.CreateBuilder(args);

// Controllers (API)
builder.Services.AddControllers();

// EF Core DbContext
builder.Services.AddDbContext<GJDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger (testing)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (so your HTML/JS frontend can call the API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("DevCors");

app.UseAuthorization();

app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");
