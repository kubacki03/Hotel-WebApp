using System.ComponentModel.DataAnnotations;
using WebApplication11.Areas.Identity.Data;

namespace WebApplication11.Models
{
    public class Reservation
    {

        [Key]
        public int ReservationId { get; set; }
        
        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public ICollection<Room> Rooms { get; set; }

        public float Price { get; set; }
    }
}
