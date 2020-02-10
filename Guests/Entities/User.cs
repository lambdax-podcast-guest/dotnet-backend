namespace Guests.Entities
{
    public class User
    {
        public int Id{get;set;}
        public string UserName{get;set;}
        public string FirstName{get;set;}
        public string LastName{get;set;}
        public byte[] PassHash{get;set;}
        public byte[] PassSalt{get;set;}
    }
}