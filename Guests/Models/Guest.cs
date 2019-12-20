using System;

namespace Guests.Models
{
    public class Guest
    {
        public Guest() => Id = new Guid();
        public Guid Id { get; set; }

        public string Name { get; set;}

        public string Email { get; set;}
    }
}