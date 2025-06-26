using System.ComponentModel.DataAnnotations;

namespace My_Car_rental.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; } // Link to a car photo
        public string? Brand { get; set; }

        public string? Model { get; set; }

        public int Year { get; set; }

        public string? Status { get; set; }  // e.g. "Available", "Rented"

        public bool IsAvailable { get; set; }  // true if available to book

        public string? Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal PricePerDay { get; set; }

        public string? Transmission { get; set; } // npr. Automatik
        public string? FuelType { get; set; }     // npr. Dizel
        public int? Seats { get; set; }           // Broj sedišta
        public string? Color { get; set; }        // npr. Crvena
    }
}