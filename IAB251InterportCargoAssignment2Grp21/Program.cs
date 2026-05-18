using IAB251InterportCargoAssignment2Grp21.Application.Interfaces;
using IAB251InterportCargoAssignment2Grp21.Application.Services;
using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Interfaces;
using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Services;
using IAB251InterportCargoAssignment2Grp21.DataAccess.ApiClients;
using IAB251InterportCargoAssignment2Grp21.DataAccess.Interfaces;
using IAB251InterportCargoAssignment2Grp21.DataAccess.Repositories;



var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();

// Session support for prototype employee login.
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Allows the HR API client to create HttpClient instances.
builder.Services.AddHttpClient();

// Layered architecture services.
builder.Services.AddScoped<IHrApiClient, HrApiClient>();
builder.Services.AddScoped<IEmployeeLoginAppService, EmployeeLoginAppService>();
builder.Services.AddScoped<IEmployeeLoginService, EmployeeLoginService>();
builder.Services.AddSingleton<ILocalEmployeeCredentialRepository, InMemoryEmployeeCredentialRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
