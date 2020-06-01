using System.Collections;
using System.Collections.Generic;
using Guests.Models.Inputs;

namespace Guests.Tests
{
    /// <summary>
    /// IEnumerable to be used in the bad password tests. GetEnumerator returns the users with the bad passwords, and the expected error message for each
    /// </summary>
    public class BadPasswordUsers : IEnumerable<object[]>
    {
        // Get Enumerator returns our list of bad password users
        // The second item on the new object[] is the expected error message for each type of bad user
        public IEnumerator<object[]> GetEnumerator()
        {
            string[] roles = new string[] { "Guest" };
            // password too short
            yield return new object[] { new RegisterInput() { FirstName = "Password", LastName = "Validation", Roles = roles, Email = "PassVal1@yahoo.com", Password = "Tes1!" }, "PasswordTooShort" };
            // no uppercase letter
            yield return new object[] { new RegisterInput() { FirstName = "Password", LastName = "Validation", Roles = roles, Email = "PassVal2@yahoo.com", Password = "test1!" }, "PasswordRequiresUpper" };
            // no lowercase letter
            yield return new object[] { new RegisterInput() { FirstName = "Password", LastName = "Validation", Roles = roles, Email = "PassVal3@yahoo.com", Password = "TEST1!" }, "PasswordRequiresLower" };
            // no number
            yield return new object[] { new RegisterInput() { FirstName = "Password", LastName = "Validation", Roles = roles, Email = "PassVal4@yahoo.com", Password = "Testt!" }, "PasswordRequiresDigit" };
            // no non alphanumeric char
            yield return new object[] { new RegisterInput() { FirstName = "Password", LastName = "Validation", Roles = roles, Email = "PassVal5@yahoo.com", Password = "Test11" }, "PasswordRequiresNonAlphanumeric" };

        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
