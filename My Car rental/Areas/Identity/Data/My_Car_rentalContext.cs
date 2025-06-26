using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using My_Car_rental.Areas.Identity.Data; // Where My_Car_rentalUser is defined
using My_Car_rental.Models;              // Where the Car class is defined

namespace My_Car_rental.Data
{
    // The context inherits from IdentityDbContext<TUser>,
    // which provides all the AspNet* tables (Users, Roles, Claims, etc.)
    public class My_Car_rentalContext : IdentityDbContext<My_Car_rentalUser>
    {
        // Constructor that receives options and passes them to the base class
        public My_Car_rentalContext(DbContextOptions<My_Car_rentalContext> options)
            : base(options)
        {
        }

        // Add your own DbSet<Car> so EF can generate a Cars table
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        // Override OnModelCreating to ensure correct Identity configuration
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // You can fine-tune table names, configure relationships, indexes, etc. here if needed
        }
    }
}