using System;

namespace Guests.Models
{
    public class Guest
    {
        
        public int Id { get; set; }

        public string Name { get; set;}

        public string Email { get; set;}

        internal object MapGuestsResponse()
        {
            throw new NotImplementedException();
        }
    }
}