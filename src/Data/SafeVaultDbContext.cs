using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SafeVault.Data;

public sealed class SafeVaultDbContext : IdentityDbContext<IdentityUser>
{
    public SafeVaultDbContext(DbContextOptions<SafeVaultDbContext> options) : base(options) { }
}
