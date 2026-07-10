using Microsoft.EntityFrameworkCore;
using VehicleCRM.Infrastructure.Persistence.Contexts;
using VehicleCRM.Infrastructure.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<VehicleCrmDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VehicleCrmConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await app.Services.ApplyMigrationsAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
