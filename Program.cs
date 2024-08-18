using DeviceContext;
using MacSave.Funcs;
using MacSave.Funcs.RegexSanitizer;
using MainDatabaseContext;
using MethodsFuncs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DeviceDb>(opt =>
    opt.UseSqlite("Data Source=Database/Workers.db")
);
builder.Services.AddDbContext<MainDatabase>(opt =>
    opt.UseSqlite("Data Source=Database/MainDatabase.db")
);

builder.Services.AddCors();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllers();
builder.Services.AddTransient<RegexService>();
builder.Services.AddTransient<DatabaseTasks>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var methods = new Methods();
methods.CheckAndCreateFolderIfNotExist();

var app = builder.Build();
app.UseRouting();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});
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
