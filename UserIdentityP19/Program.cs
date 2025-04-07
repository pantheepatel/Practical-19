using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserIdentityP19.Models;
using UserIdentityP19;
using UserIdentityP19.Mapping;
using UserIdentityP19.Repository.AuthRepo;
using UserIdentityP19.Repository.StudentRepo;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<Role>>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    dbContext.Database.Migrate(); // Ensure the latest migration is applied
    SeedRolesAsync(roleManager, userManager).GetAwaiter().GetResult();
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
async Task SeedRolesAsync(RoleManager<Role> roleManager, UserManager<User> userManager)
{
    string[] roleNames = { "Admin", "User" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var role = new Role { Name = roleName };
            await roleManager.CreateAsync(role);
        }
    }

    // Ensure an Admin user exists (Optional)
    var adminEmail = "admin@gmail.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var newAdmin = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User",
            MobileNumber = "1234567890"
        };

        var result = await userManager.CreateAsync(newAdmin, "Admin@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }
}
