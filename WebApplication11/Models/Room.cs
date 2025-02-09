using System.ComponentModel.DataAnnotations;

namespace WebApplication11.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int MaxPeople { get; set; }

        [Required]
        public int Price { get; set; }



        public ICollection<Reservation> Reservations { get; set; }

    }
}
