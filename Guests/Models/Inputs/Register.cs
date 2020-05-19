using System.ComponentModel.DataAnnotations;
namespace Guests.Models.Inputs
{
    public class RegisterInput
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string[] Roles { get; set; }
        public RegisterInput() { }
    }
}