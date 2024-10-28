using FinanceManager.BL.Extensions;
using FinanceManager.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5009);
});

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDataServices(connectionString);
builder.Services.AddBLServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinanceManager API v1"));
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

app.Run();
