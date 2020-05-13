using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Guests.Models
{
    // inherit from timestamp entity to get timestamps
    public class Invitation : AppUserContext.TimestampEntity
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Podcast")]
        public int PodcastId { get; set; }
        public Podcast Podcast { get; set; }

        [Required]
        [ForeignKey("GuestUser")]
        public string GuestId { get; set; }
        public AppUser GuestUser { get; set; }

        [Required]
        [ForeignKey("HostUser")]
        public string HostId { get; set; }
        public AppUser HostUser { get; set; }

        [Required]
        public string RequestType { get; set; }
        public string InvitationText { get; set; }
    }
}
