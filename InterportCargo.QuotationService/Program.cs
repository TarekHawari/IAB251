using InterportCargo.QuotationService.Data;
using InterportCargo.QuotationService.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuotationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("QuotationDatabase")));

builder.Services.AddHttpClient("notification-api", client =>
{
    var notificationServiceUrl = builder.Configuration["ServiceUrls:NotificationService"];

    if (string.IsNullOrWhiteSpace(notificationServiceUrl))
    {
        throw new InvalidOperationException("NotificationService URL is missing.");
    }

    client.BaseAddress = new Uri(notificationServiceUrl);
});

builder.Services.AddScoped<NotificationServiceClient>();

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
    foreach (var loaderException in ex.LoaderExceptions)
    {
        Console.WriteLine(loaderException?.Message);
    }

    throw;
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<QuotationDbContext>();
    db.Database.EnsureCreated();
}

app.Run();