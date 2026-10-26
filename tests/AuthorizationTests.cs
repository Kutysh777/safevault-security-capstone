using Microsoft.AspNetCore.Authorization;
using SafeVault.Controllers;

namespace SafeVault.Tests;

public sealed class AuthorizationTests
{
    [Fact]
    public void AdminControllerRequiresAdminPolicy()
    {
        var attribute = typeof(AdminController)
            .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true)
            .Cast<AuthorizeAttribute>()
            .Single();
        Assert.Equal("AdminOnly", attribute.Policy);
    }
}
