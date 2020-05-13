using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Guests.Models
{
    // inherit from timestamp entity to get timestamps
    public class PodcastTopic : AppUserContext.TimestampEntity
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Podcast")]
        public int PodcastId { get; set; }
        public Podcast Podcast { get; set; }
        [Required]
        [ForeignKey("Topic")]
        public int TopicId { get; set; }
        public Topic Topic { get; set; }

    }
}
