using Airways.Data;
using Airways.Domain.DataDB;
using Airways.Repository.Interfaces;
using Airways.Repository;
using Airways.Services.Interfaces;
using Airways.Services;
using Airways.Util.Mail.Interfaces;
using Airways.Util.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewOptions(options =>
    {
        options.HtmlHelperOptions.ClientValidationEnabled = true;
    });

// Add Dependency Injections
builder.Services.AddScoped<ICustomerProfileDAO, CustomerProfileDAO>();
builder.Services.AddScoped<ICustomerProfileService, CustomerProfileService>();
builder.Services.AddScoped<ICityDAO, CityDAO>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICityDAO, CityDAO>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IConnectionDAO, ConnectionDAO>();
builder.Services.AddScoped<IConnectionService, ConnectionService>();
builder.Services.AddScoped<IFlightDAO, FlightDAO>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IMealDAO, MealDAO>();
builder.Services.AddScoped<IMealService, MealService>();
builder.Services.AddScoped<IBookingDAO, BookingDAO>();
builder.Services.AddScoped<ICustomerPrefDAO, CustomerPrefDAO>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICustomerPrefService, CustomerPrefService>();
builder.Services.AddScoped<IRapidApiDestinationDAO, RapidApiDestinationDAO>();
builder.Services.AddScoped<IRapidApiDestinationService, RapidApiDestinationService>();

// EmailSettings
var emailSettings = new Airways.Util.Mail.EmailSettings();
builder.Configuration.GetSection("EmailSettings").Bind(emailSettings);

builder.Services.AddSingleton(emailSettings);

builder.Services.AddSingleton<Airways.Util.Mail.Interfaces.IEmailSend, Airways.Util.Mail.EmailSend>();

// RapidAPI settings
var rapidApiDestinationSettings = new Airways.Domain.RapidApiDestinationSettings();
builder.Configuration.GetSection("RapidApiDestinationSettings").Bind(rapidApiDestinationSettings);
builder.Services.AddSingleton(rapidApiDestinationSettings);

// Register HttpClient for API calls
builder.Services.AddHttpClient();

// DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<FlightBookingDBContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
    b => b.MigrationsAssembly("Airways"))); 

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
   
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
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
app.MapRazorPages();

app.Run();