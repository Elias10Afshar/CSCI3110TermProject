using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CSCI3110TermProject.Data;

// Create the WebApplication builder
var builder = WebApplication.CreateBuilder(args);

// 1. EF Core & Identity
// Register our ApplicationDbContext using SQL Server and the connection string from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Register ASP.NET Core Identity with our custom ApplicationUser
builder.Services.AddDefaultIdentity<ApplicationUser>(opts =>
    opts.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
// Customize the cookie paths for login and access‑denied redirects
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});


// 2. MVC + Razor Pages
// Add support for controllers with views (classic MVC)
builder.Services.AddControllersWithViews();
// Add support for Razor Pages
builder.Services.AddRazorPages();

// register API controllers
builder.Services.AddControllers();    

var app = builder.Build();

// 3. Error handling for non-Dev
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 4. Standard middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// enable [ApiController] endpoints
app.MapControllers();


// 5. Map endpoints
// Map the default MVC route: /{controller=Home}/{action=Index}/{id?}
app.MapControllerRoute(
    "default", "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();