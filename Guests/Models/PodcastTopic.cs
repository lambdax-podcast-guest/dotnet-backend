using System;
using System.ComponentModel.DataAnnotations;

namespace Guests.Models
{
    // inherit from timestamp entity to get timestamps
    public class PodcastTopic : GuestsContext.TimestampEntity
    {
        public int Id { get; set; }

        [Required]
        public int PodcastId { get; set; }
        public Podcast Podcast { get; set; }
        [Required]
        public int TopicId { get; set; }
        public Topic Topic { get; set; }

    }
}
