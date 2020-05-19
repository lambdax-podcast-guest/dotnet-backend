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
    public Mocks(AppUserContext DbContext)
    {
        // mock userManager
        userManager = CreateUserManager(DbContext);

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
    public UserManager<AppUser> CreateUserManager(AppUserContext DbContext)
    {
        // mock userManager
        var userStoreMock = new Mock<IUserStore<AppUser>>().Object;
        var passwordHasher = new PasswordHasher<AppUser>();
        var userManagerMock = new Mock<UserManager<AppUser>>(userStoreMock, null, null, null, null, null, null, null, null);
        // set up userManager functions
        // TODO
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback((AppUser user, string password) =>
        {
            user.PasswordHash = passwordHasher.HashPassword(user, password);
            DbContext.Add(user);
            DbContext.SaveChanges();
        });
        userManagerMock.Setup(x => x.Users).Returns(DbContext.Users);
        // https://medium.com/@samueleresca/unit-testing-asp-net-core-identity-e2b18254cc8a
        // userManagerMock.Setup(x => x.Users).Returns(users);
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