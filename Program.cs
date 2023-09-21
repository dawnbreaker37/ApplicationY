using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient(typeof(IBase<>), typeof(Base<>));
builder.Services.AddTransient<IUser, UserRepository>();
builder.Services.AddTransient<INotifications, NotificationsRepository>();
builder.Services.AddTransient<IAccount, AccountRepository>();
builder.Services.AddTransient<IMailService, SendEmailRepository>();
builder.Services.AddIdentity<User, IdentityRole<int>>(Opt =>
{
    Opt.Password.RequiredUniqueChars = 8;
    Opt.Password.RequiredUniqueChars = 0;
    Opt.Password.RequireUppercase = false;
    Opt.Password.RequireLowercase = false;
    Opt.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<Context>().AddRoles<IdentityRole<int>>().AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);
builder.Services.AddDbContext<Context>(Opt => Opt.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
