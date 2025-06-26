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


    app.Run();
}