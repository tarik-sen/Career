using Career.Authorization;
using Career.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages( options =>
{
    options.Conventions.AuthorizeAreaFolder("Admin", "/", "RequireAdministratorAccount");
    options.Conventions.AuthorizeAreaFolder("Profile", "/", "RequireUserAccount");
});
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorAccount", policy => policy.RequireRole(Constants.AdministratorRole));
    options.AddPolicy("RequireUserAccount", policy => policy.RequireRole(Constants.UserRole));
    options.AddPolicy("RequireAnonymous", policy => policy.RequireAssertion(context => 
    {
        return !context.User.Identity.IsAuthenticated;
    }));

    options.AddPolicy("RequireUserOrAnonymous", policy => policy.RequireAssertion(context =>
    {
        return !context.User.Identity.IsAuthenticated || context.User.IsInRole(Constants.UserRole);
    }));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var adminPW = builder.Configuration.GetValue<string>("AdminPW");
    var userPW = builder.Configuration.GetValue<string>("UserPW");

    await SeedData.Initialize(services, adminPW!, userPW!);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();
