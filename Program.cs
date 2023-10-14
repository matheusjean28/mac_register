using Microsoft.EntityFrameworkCore;
using DeviceContext;
using MethodsFuncs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DeviceDb>(opt => opt.UseSqlite("Data Source=C:\\dev\\database\\Workers.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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




app.Run();
