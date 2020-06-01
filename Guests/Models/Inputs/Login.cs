using System.ComponentModel.DataAnnotations;
namespace Guests.Models.Inputs
{
    public class LoginInput
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public LoginInput() { }
    }
}