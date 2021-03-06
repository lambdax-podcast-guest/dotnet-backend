using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guests.Models
{
    // inherit from timestamp entity to get timestamps
    public class GuestTopic : AppUserContext.TimestampEntity
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string GuestId { get; set; }
        public AppUser User { get; set; }
        [Required]
        [ForeignKey("Topic")]
        public int TopicId { get; set; }
        public Topic Topic { get; set; }

    }
}
