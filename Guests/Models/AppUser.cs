using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Guests.Models
{
    /// <summary>Represents a user on the database.</summary>
    public class AppUser : IdentityUser
    {
        /// <summary>First name of user</summary>
        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }

        /// <summary>Last name of user</summary>
        [Required]
        [StringLength(200)]
        public string LastName { get; set; }

        /// <summary>Url representing the user's avatar</summary>
        [Url]
        public string AvatarUrl { get; set; }

        /// <summary>Catchy headline for user</summary>
        [StringLength(200)]
        public string HeadLine { get; set; }

        /// <summary>String representing user's location</summary>
        /// <value>"City, ST"</value>
        [StringLength(200)]
        public string Location { get; set; }

        public string Bio { get; set; }

        public string[] Languages { get; set; }

        public List<GuestTopic> GuestTopics { get; set; }

        public List<PodcastHost> PodcastHosts { get; set; }

        public List<PodcastGuest> PodcastGuests { get; set; }

        public List<Invitation> Invitations { get; set; }
    }
}
