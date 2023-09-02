using Microsoft.EntityFrameworkCore;
using DeviceModel;
using DeviceContext;
using Microsoft.Extensions.DependencyModel;
using System.Runtime.CompilerServices;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DeviceDb>(opt => opt.UseSqlite("Data Source=Mac.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();
app.UseStaticFiles();


app.MapGet("/mac", async (DeviceDb db) =>
    await db.Devices.ToListAsync());


app.MapPost("/mac", async (Device device, DeviceDb db) =>
{
    db.Devices.Add(device);
    await db.SaveChangesAsync();

    return Results.Created("/mac", device);
});

app.Run();