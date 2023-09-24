using Microsoft.EntityFrameworkCore;
using DeviceModel;
using DeviceContext;
using ModelsFileToUpload;
using MethodsFuncs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DeviceDb>(opt => opt.UseSqlite("Data Source=Mac.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var methods = new Methods();
methods.CheckAndCreateFolderIfNotExist();

var app = builder.Build();
app.UseRouting();
app.MapControllers();

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


app.MapGet("/uploads", async (DeviceDb db) =>
    await db.FilesUploads.ToListAsync());


app.MapPost("/mac", async (Device device, DeviceDb db) =>
{
    db.Devices.Add(device);
    await db.SaveChangesAsync();
    return Results.Created("/mac", device);
});


app.Run();
