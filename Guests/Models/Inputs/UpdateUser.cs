using System.ComponentModel.DataAnnotations;

namespace Guests.Models.Inputs
{
    public class UpdateUser
    {
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
    }
}