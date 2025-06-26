using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Car_rental.Data;
using My_Car_rental.Models;
using System.Diagnostics;

namespace My_Car_rental.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly My_Car_rentalContext _context;

        public HomeController(ILogger<HomeController> logger, My_Car_rentalContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Show a few available cars on the home page
        public async Task<IActionResult> Index()
        {
            var topCars = await _context.Cars
                .Where(c => c.IsAvailable)
                .Take(3)
                .ToListAsync();

            return View(topCars);
        }

        // Show all cars
        public async Task<IActionResult> Cars()
        {
            var cars = await _context.Cars.ToListAsync();
            return View(cars);
        }

        // Show details for a specific car
        public async Task<IActionResult> Details(int id)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            // EXPICIT path to the view
            return View("~/Views/Home/Details.cshtml", car);
        }

        // Book a car by ID and update its availability
        public async Task<IActionResult> Book(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            if (!car.IsAvailable)
            {
                ViewBag.Message = "Sorry, this car is already booked.";
                return View();
            }

            car.IsAvailable = false;
            car.Status = "Booked";

            await _context.SaveChangesAsync();

            ViewBag.Message = $"You have successfully booked: {car.Brand} {car.Model} ({car.Year})!";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}