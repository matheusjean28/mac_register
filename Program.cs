using Microsoft.EntityFrameworkCore;
using DeviceModel;
using DeviceContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DeviceDb>(opt => opt.UseSqlite("Data Source=Mac.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});


app.MapGet("/mac", async (DeviceDb db) =>
    await db.Devices.ToListAsync());

app.MapPost("/mac", async (Device device, DeviceDb db) =>
{
    db.Devices.Add(device);
    await db.SaveChangesAsync();
    return Results.Created("/mac", device);
});


app.Run();
