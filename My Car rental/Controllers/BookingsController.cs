using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Car_rental.Models;
using My_Car_rental.Areas.Identity.Data;
using System.Linq;
using System.Threading.Tasks;
using My_Car_rental.Data;

namespace My_Car_rental.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookingsController : Controller
    {
        private readonly My_Car_rentalContext _context;

        public BookingsController(My_Car_rentalContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Car)
                .ToListAsync();
            return View(bookings);
        }
    }
} 