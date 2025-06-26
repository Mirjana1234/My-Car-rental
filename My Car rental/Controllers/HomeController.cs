using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Car_rental.Data;
using My_Car_rental.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

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

        [HttpPost]
        public async Task<IActionResult> BookModal(int CarId, string UserEmail, DateTime StartDate, DateTime EndDate, string TotalPrice)
        {
            var car = await _context.Cars.FindAsync(CarId);
            if (car == null || !car.IsAvailable)
            {
                TempData["BookingError"] = "Car is not available.";
                return RedirectToAction("Cars");
            }

            // Calculate price (parse from TotalPrice or recalculate)
            var days = (EndDate - StartDate).Days + 1;
            var price = car.PricePerDay * days;

            // Save booking
            var booking = new Booking
            {
                CarId = CarId,
                UserId = User.Identity.IsAuthenticated ? User.Identity.Name : UserEmail,
                StartDate = StartDate,
                EndDate = EndDate,
                Price = price,
                Status = "Confirmed"
            };
            _context.Bookings.Add(booking);

            // Update car status
            car.IsAvailable = false;
            car.Status = "Booked";
            await _context.SaveChangesAsync();

            // Send confirmation email
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("your-email@gmail.com", "your-app-password"),
                    EnableSsl = true,
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("your-email@gmail.com"),
                    Subject = "Car Booking Confirmation",
                    Body = $"Thank you for booking {car.Brand} {car.Model} from {StartDate:yyyy-MM-dd} to {EndDate:yyyy-MM-dd}. Total price: {price:C}.",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(UserEmail);
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch
            {
                // Ignore email errors for now
            }

            TempData["BookingSuccess"] = $"Booking confirmed for {car.Brand} {car.Model}. Confirmation sent to {UserEmail}.";
            return RedirectToAction("Cars");
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