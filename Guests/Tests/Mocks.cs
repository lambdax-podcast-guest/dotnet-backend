using Moq;
using Microsoft.AspNetCore.Identity;
using Guests.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
public class Mocks
{

    public RoleManager<IdentityRole> roleManager { get; }
    public UserManager<AppUser> userManager { get; }

    public SignInManager<AppUser> signInManager { get; }
    public Mocks()
    {
        // mock userManager
        userManager = CreateUserManager();

        // mock roleManager
        roleManager = CreateRoleManager();

        // mock signManager
        signInManager = CreateSignInManager();

    }
    public RoleManager<IdentityRole> CreateRoleManager()
    {
        // mock roleManager
        var roleStoreMock = new Mock<IRoleStore<IdentityRole>>().Object;
        var roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStoreMock, null, null, null, null);
        // set up roleManager functions
        // RoleExistsAsync should return true if it is called with either guest or host in any case
        roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsRegex("guest||host", RegexOptions.IgnoreCase))).ReturnsAsync(true);
        return roleManagerMock.Object;
    }
    public UserManager<AppUser> CreateUserManager()
    {
        // mock userManager
        var userStoreMock = new Mock<IUserStore<AppUser>>().Object;
        var userManagerMock = new Mock<UserManager<AppUser>>(userStoreMock, null, null, null, null, null, null, null, null);
        // set up userManager functions
        // TODO
        // set MockUserManager field
        return userManagerMock.Object;
    }

    public SignInManager<AppUser> CreateSignInManager()
    {
        // mock signInManager
        var contextAccessor = new Mock<IHttpContextAccessor>().Object;
        var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object;
        var signManagerMock = new Mock<SignInManager<AppUser>>(userManager, contextAccessor, userPrincipalFactory, null, null, null, null);
        return signManagerMock.Object;
    }
}