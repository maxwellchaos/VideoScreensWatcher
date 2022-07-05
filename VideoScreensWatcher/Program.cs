using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VideoScreensWatcher.Data;
using VideoScreensWatcher.Messages;
using VideoScreensWatcher.Models;
using VideoScreensWatcher.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<VideoScreensWatcherContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VideoScreensWatcherContext") ?? throw new InvalidOperationException("Connection string 'VideoScreensWatcherContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<DatabaseUpdateService, DatabaseUpdateService>();

var app = builder.Build();

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
    pattern: "{controller=Computers}/{action=Index}/{id?}");

app.Services.GetService<DatabaseUpdateService>()?.HasNewStatuses();
app.Run();
