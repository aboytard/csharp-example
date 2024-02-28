using AlbanWebApplication.Data;
using AlbanWebApplication.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ProfessionelContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ProfessionelContext") ?? throw new InvalidOperationException("Connection string 'ProfessionelContext' not found.")));
builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AlbanWebApplicationContext") ?? throw new InvalidOperationException("Connection string 'AlbanWebApplicationContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
