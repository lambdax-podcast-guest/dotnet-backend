using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guests.Models
{
    // inherit from timestamp entity to get timestamps
    public class Podcast : AppUserContext.TimestampEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }
        public List<PodcastTopic> PodcastTopics { get; set; }

        public List<PodcastHost> PodcastHosts { get; set; }

    }
}
