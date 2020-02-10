using System.ComponentModel.DataAnnotations;

namespace Guests.Models.Users
{
    public class AuthenticateModel{
        [Required]
        public string Username{get;set;}
        [Required]
        public string Password{get;set;}
    }
}