using InterportCargo.NotificationService.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("NotificationDatabase")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

try
{
    app.MapControllers();
}
catch (ReflectionTypeLoadException ex)
{
    Console.WriteLine("===== TYPE LOAD ERROR =====");
    Debug.WriteLine("===== TYPE LOAD ERROR =====");

    foreach (var loaderException in ex.LoaderExceptions)
    {
        Console.WriteLine(loaderException?.ToString());
        Debug.WriteLine(loaderException?.ToString());
    }

    Console.WriteLine("===== END TYPE LOAD ERROR =====");
    Debug.WriteLine("===== END TYPE LOAD ERROR =====");

    throw;
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
    db.Database.EnsureCreated();
}

app.Run();