using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Car_rental.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; }
        [ForeignKey("CarId")]
        public Car Car { get; set; }

        [Required]
        public string UserId { get; set; } // Assuming Identity user string key
        // No navigation property for user yet, can be added if needed

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string Status { get; set; } // e.g. "Pending", "Confirmed", "Cancelled"
    }
} 