using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Guests.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(200)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string AvatarUrl { get; set; }

        [StringLength(200)]
        public string HeadLine { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        public string Bio { get; set; }

        public string Languages { get; set; }

        public List<GuestTopic> GuestTopics { get; set; }

    }
}
