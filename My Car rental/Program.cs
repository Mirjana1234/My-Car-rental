using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using My_Car_rental.Areas.Identity.Data;  // My_Car_rentalUser
using My_Car_rental.Data;                // My_Car_rentalContext

var builder = WebApplication.CreateBuilder(args);
builder.Environment.EnvironmentName = Environments.Development;

// 1) L�s in connection string fr�n appsettings.json
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 2) Registrera din enda DbContext (My_Car_rentalContext) med SQLite
builder.Services.AddDbContext<My_Car_rentalContext>(options =>
    options.UseSqlite(connectionString));

// 3) Konfigurera Identity att anv�nda My_Car_rentalUser och My_Car_rentalContext
builder.Services.AddIdentity<My_Car_rentalUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<My_Car_rentalContext>();

// 4) L�gg till MVC + Razor Pages (kr�vs f�r Identity UI)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, NoOpEmailSender>();
builder.Environment.EnvironmentName = Environments.Development;
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Viktigt: f�rst Authentication, sedan Authorization
app.UseAuthentication();
app.UseAuthorization();

// 6) Route f�r vanliga controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 7) Route f�r Razor Pages (Identity-sidorna)
app.MapRazorPages();

// 8) Skapa roller (om inte redan finns)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRoles.InitializeAsync(services);
}

// 9) Tilldela anv�ndaren rollen "Admin"
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<My_Car_rentalUser>>();

    var user = await userManager.FindByEmailAsync("mirjanavasic91@gmail.com");
    if (user != null)
    {
        var isInRole = await userManager.IsInRoleAsync(user, "Admin");
        if (!isInRole)
        {
            await userManager.AddToRoleAsync(user, "Admin");
            Console.WriteLine("User assigned to Admin role.");
        }
    }
}
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<My_Car_rentalContext>();

    // Seed default cars if none exist
    if (!context.Cars.Any())
    {
        context.Cars.AddRange(
            new My_Car_rental.Models.Car
            {
                Brand = "Volvo",
                Model = "XC 60",
                Year = 2020,
                Status = "Available",
                IsAvailable = true,
                PricePerDay = 500,
                Transmission = "Automatic",
                FuelType = "Diesel",
                Seats = 5,
                Color = "White",
                Description = "Spacious and comfortable SUV.",
                ImageUrl = "/images/volvo.jpg"
            },
            new My_Car_rental.Models.Car
            {
                Brand = "BMW",
                Model = "3",
                Year = 2018,
                Status = "Available",
                IsAvailable = true,
                PricePerDay = 450,
                Transmission = "Manual",
                FuelType = "Petrol",
                Seats = 5,
                Color = "Blue",
                Description = "Sporty sedan with great performance.",
                ImageUrl = "/images/bmw.jpg"
            },
            new My_Car_rental.Models.Car
            {
                Brand = "Toyota",
                Model = "Yaris",
                Year = 2020,
                Status = "Available",
                IsAvailable = true,
                PricePerDay = 300,
                Transmission = "Automatic",
                FuelType = "Hybrid",
                Seats = 5,
                Color = "Red",
                Description = "Economical and reliable compact car.",
                ImageUrl = "/images/toyota.jpg"
            }
        );
        context.SaveChanges();
    }

    // One-time update for existing cars with missing fields
    var volvo = context.Cars.FirstOrDefault(c => c.Brand == "Volvo" && c.Model == "XC 60");
    if (volvo != null)
    {
        volvo.Transmission ??= "Automatic";
        volvo.FuelType ??= "Diesel";
        volvo.Seats = volvo.Seats == 0 ? 5 : volvo.Seats;
        volvo.Color ??= "White";
        volvo.Description ??= "Spacious and comfortable SUV.";
        volvo.ImageUrl ??= "/images/volvo.jpg";
        if (volvo.PricePerDay == 0) volvo.PricePerDay = 500;
    }
    var bmw = context.Cars.FirstOrDefault(c => c.Brand == "BMW" && c.Model == "3");
    if (bmw != null)
    {
        bmw.Transmission ??= "Manual";
        bmw.FuelType ??= "Petrol";
        bmw.Seats = bmw.Seats == 0 ? 5 : bmw.Seats;
        bmw.Color ??= "Blue";
        bmw.Description ??= "Sporty sedan with great performance.";
        bmw.ImageUrl ??= "/images/bmw.jpg";
        if (bmw.PricePerDay == 0) bmw.PricePerDay = 450;
    }
    var toyota = context.Cars.FirstOrDefault(c => c.Brand == "Toyota" && c.Model == "Yaris");
    if (toyota != null)
    {
        toyota.Transmission ??= "Automatic";
        toyota.FuelType ??= "Hybrid";
        toyota.Seats = toyota.Seats == 0 ? 5 : toyota.Seats;
        toyota.Color ??= "Red";
        toyota.Description ??= "Economical and reliable compact car.";
        toyota.ImageUrl ??= "/images/toyota.jpg";
        if (toyota.PricePerDay == 0) toyota.PricePerDay = 300;
    }
    context.SaveChanges();
}

app.Run();