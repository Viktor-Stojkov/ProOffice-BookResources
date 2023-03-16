using Microsoft.EntityFrameworkCore;
using ProOffice_BookResources.EmailService;
using ProOffice_BookResources.EmailService.Interface;
using ProOffice_BookResources.ProOffice_Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProOfficeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<EmailConfiguration>();


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
