using System.ComponentModel.DataAnnotations;

namespace Guests.Models.Inputs
{
    public class PodcastInput
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string HeadLine { get; set; }

        [Required]
        public string[] Topics { get; set; }
    }
}
