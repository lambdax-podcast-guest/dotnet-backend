using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Guests.Models
{
    // inherit from timestamp entity to get timestamps
    public class PodcastTopic : AppUserContext.TimestampEntity
    {
        public int Id { get; set; }

        [Required]
        public int PodcastId { get; set; }

        [JsonIgnore]
        public virtual Podcast Podcast { get; set; }

        [Required]
        public int TopicId { get; set; }

        [JsonIgnore]
        public virtual Topic Topic { get; set; }

    }
}
