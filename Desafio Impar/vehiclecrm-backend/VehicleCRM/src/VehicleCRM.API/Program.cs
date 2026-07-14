using Microsoft.EntityFrameworkCore;
using VehicleCRM.API.Middleware;
using VehicleCRM.Infrastructure.Persistence.Contexts;
using VehicleCRM.Infrastructure.Persistence.Extensions;
using VehicleCRM.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VehicleCrmDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VehicleCrmConnection")));
builder.Services.AddVehicleCrmServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await app.Services.ApplyMigrationsAsync();
await app.Services.SeedDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable exception handling middleware
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
