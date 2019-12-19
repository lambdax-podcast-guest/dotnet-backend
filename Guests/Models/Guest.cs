// import needed for the required and string length attributes 
using System.ComponentModel.DataAnnotations; 

namespace Guests.Models{
    public class Guest
    {
        public int Id { get; set; }

        [Required] //makes the name a required field
        [StringLength(200)] //string length max of 200 characters
        public string Name { get; set;}

        [Required] //makes the email a required field
        [StringLength(200)] //string length max of 200 characters
        public string Email { get; set;}
    }
}