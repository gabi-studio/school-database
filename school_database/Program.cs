using System;
using School.Models;
using School.Controllers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Database
builder.Services.AddScoped<SchoolDbContext>();

// add TeacherAPI
builder.Services.AddScoped<TeacherAPIController>();

// add StudentAPI
builder.Services.AddScoped<StudentAPIController>();

// add CourseAPI
builder.Services.AddScoped<CourseAPIController>();


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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
