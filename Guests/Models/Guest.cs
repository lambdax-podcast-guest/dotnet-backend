using System.ComponentModel.DataAnnotations;

namespace Guests.Models
{
    public class Guest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

    }
}