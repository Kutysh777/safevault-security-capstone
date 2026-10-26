using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Security;

namespace SafeVault.Controllers;

[ApiController]
[Route("api/account")]
public sealed class AccountController(
    UserManager<IdentityUser> users,
    SignInManager<IdentityUser> signIn,
    IJwtTokenService tokens) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!InputValidator.IsSafeUsername(request.Username) ||
            !InputValidator.IsValidEmail(request.Email))
            return BadRequest("Invalid registration data.");

        var user = new IdentityUser { UserName = request.Username, Email = request.Email };
        var result = await users.CreateAsync(user, request.Password);
        if (!result.Succeeded) return BadRequest("Registration failed.");
        await users.AddToRoleAsync(user, "User");
        return Created("/api/account", new { user.Id, user.UserName });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await users.FindByNameAsync(request.Username);
        if (user is null) return Unauthorized();
        var result = await signIn.CheckPasswordSignInAsync(user, request.Password, true);
        if (!result.Succeeded) return Unauthorized();
        var roles = await users.GetRolesAsync(user);
        return Ok(new { access_token = tokens.Create(user.Id, user.UserName!, roles) });
    }
}

public sealed record RegisterRequest(string Username, string Email, string Password);
public sealed record LoginRequest(string Username, string Password);
