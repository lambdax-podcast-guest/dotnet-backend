using Guests.Models.Inputs;

namespace Guests.Tests
{
    // -------------------------------------------------------------------------------------------------
    /// <summary>Helpers for testing the account endpoints: Login, Register, Update and Delete User. Create an instance of this class in the database fixture to be shared with the tests</summary>
    // -------------------------------------------------------------------------------------------------
    public class AccountHelper
    {
        public int Index { get; private set; }

        public AccountHelper()
        {
            Index = 0;
        }

        /// <summary>Generates a new valid register model, with incrementing index property to make sure our unique properties are unique. Pass in your users roles as a string[]</summary>
        public RegisterInput GenerateUniqueRegisterModel(string[] roles)
        {
            RegisterInput guestUser = new RegisterInput() { FirstName = $"Unique{Index}", LastName = $"User{Index}", Roles = roles, Email = $"UniqueUser{Index}@yahoo.com", Password = $"Unique{Index}!" };
            Index++;
            return guestUser;
        }
    }
}