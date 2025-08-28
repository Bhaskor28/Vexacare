using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Vexacare.Application.Categories;
using Vexacare.Application.DoctorProfiles;
using Stripe;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Mapping;
using Vexacare.Application.ServiceTypes;
using Vexacare.Application.Users.Doctors;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Data;
using Vexacare.Infrastructure.Data.Configurations.Admin;
using Vexacare.Infrastructure.Repositories;
using Vexacare.Infrastructure.Services;
using Vexacare.Infrastructure.Services.Categories;
using Vexacare.Infrastructure.Services.DoctorProfileServices;
using Vexacare.Infrastructure.Services.LocationServices;
using Vexacare.Infrastructure.Services.ProductServices;
using Vexacare.Infrastructure.Services.ServiceTypes;
using Vexacare.Infrastructure.Services.StripeServices;


// Aliases to avoid conflict with Stripe
using AppOrderService = Vexacare.Infrastructure.Services.OrderService;
using AppProductService = Vexacare.Infrastructure.Services.ProductService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Added By Sazib

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, AppProductService>();  //change
builder.Services.AddScoped<IBenefitRepository, BenefitRepository>();

// Add AutoMapper with your assembly
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// In ConfigureServices method of Startup.cs
builder.Services.AddMemoryCache(); // Add this if not already added
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IOrderService, AppOrderService>();  //change
// Register the StripeConfigService
builder.Services.AddScoped<StripeConfigService>();
#endregion

#region Added By Bhaskor
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IDoctorProfileService, DoctorProfileService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IServiceTypeService, ServiceTypeService>();

#endregion

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
#endregion


var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await DataSeeder.SeedAdminUserAndRoleAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding admin data.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Stripe config
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
