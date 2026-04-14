using Microsoft.EntityFrameworkCore;
using TopUpMVC.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    var createUsers = "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U') " +
                      "CREATE TABLE Users (" +
                      "Id INT IDENTITY(1,1) PRIMARY KEY," +
                      "FullName NVARCHAR(100) NOT NULL," +
                      "Username NVARCHAR(50) NOT NULL," +
                      "Email NVARCHAR(255) NOT NULL," +
                      "Password NVARCHAR(100) NOT NULL," +
                      "ConfirmPassword NVARCHAR(100) NOT NULL," +
                      "Role NVARCHAR(20) NOT NULL DEFAULT 'User'," +
                      "CreatedAt DATETIME NOT NULL DEFAULT GETDATE())";

    db.Database.ExecuteSqlRaw(createUsers);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();