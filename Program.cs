using Microsoft.EntityFrameworkCore;
using RI_App.Models;
using RI_App.DataStructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register ReportIssueQueue as a singleton
// This ensures the queue persists in memory across requests
builder.Services.AddSingleton<ReportIssueQueue>();
// Register LocalEventManager as a singleton
// This ensures the event persists in memory across requests
builder.Services.AddSingleton<LocalEventManager>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
