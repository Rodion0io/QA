using System.Globalization;
using Microsoft.EntityFrameworkCore;
using todo.Datas;
using todo.Service;
using todo.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var cultureInfo = new CultureInfo("ru-RU");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connection));
// builder.Services.AddDbContext<DataTesterContext>(options => options.UseNpgsql(connection));
var testerConnection = builder.Configuration.GetConnectionString("TesterConnection");
builder.Services.AddDbContext<DataTesterContext>(options => options.UseNpgsql(testerConnection));


builder.Services.AddScoped<MacrosService>();
builder.Services.AddScoped<IAppService, AppService>();

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }