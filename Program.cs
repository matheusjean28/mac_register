using Microsoft.EntityFrameworkCore;
using DeviceModel;
using DeviceContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DeviceDb>(opt => opt.UseInMemoryDatabase("Macs"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/mac", async (DeviceDb db) =>
    await db.Devices.ToListAsync());

app.MapPost("/mac", async (Device device, DeviceDb db) =>
{
    db.Devices.Add(device);
    await db.SaveChangesAsync();

    return Results.Created("/mac",device);
});

app.Run();