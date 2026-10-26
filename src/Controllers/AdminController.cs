using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SafeVault.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "AdminOnly")]
public sealed class AdminController : ControllerBase
{
    [HttpGet("audit")]
    public IActionResult Audit() => Ok(new { status = "authorized" });
}
