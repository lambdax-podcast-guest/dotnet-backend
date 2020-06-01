using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Guests.Models
{
    // inherit from timestamp entity to get timestamps
    public class PodcastGuest : AppUserContext.TimestampEntity
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Podcast")]
        public int PodcastId { get; set; }
        public Podcast Podcast { get; set; }
        [Required]
        [ForeignKey("User")]
        public string GuestId { get; set; }
        public AppUser User { get; set; }

    }
}
