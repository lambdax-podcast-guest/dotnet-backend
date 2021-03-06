namespace Guests.Tests
{
    /// <summary>
    /// The expected shape of the response from the Login Request
    /// </summary>
    public class LoginOutput
    {
        public string token { get; set; }
    }

    /// <summary>
    /// The expected shape of the response from the Register Request
    /// </summary>
    public class RegisterOutput
    {
        public string id { get; set; }
        public string token { get; set; }
    }

    /// <summary>
    /// Expected shape of a bad request object (400)
    /// </summary>
    public class CustomBadRequest
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string traceId { get; set; }
        public object errors { get; set; }
    }

    /// <summary>
    /// General Error shape
    /// </summary>
    public class Errors
    {
        public string[] DuplicateEmail { get; set; }
        public string[] DuplicateUserName { get; set; }
        public string[] PasswordRequiresNonAlphanumeric { get; set; }
        public string[] PasswordRequiresDigit { get; set; }
        public string[] PasswordRequiresLower { get; set; }
        public string[] PasswordRequiresUpper { get; set; }
        public string[] PasswordTooShort { get; set; }

    }
}
