# SafeVault Security Capstone

SafeVault is a small ASP.NET Core reference application that demonstrates secure input handling, parameterized database access, password hashing, JWT authentication, and role-based authorization.

## Security controls

- Allow-list validation for usernames and standards-based email validation.
- Output encoding for untrusted text.
- Entity Framework Core queries, which parameterize user input instead of concatenating SQL.
- ASP.NET Core Identity password hashing and account lockout.
- Short-lived JWT access tokens signed with a key loaded from configuration.
- Role-based access control for administrator endpoints.
- HTTPS redirection, HSTS, a restrictive Content-Security-Policy header, and safe error responses.

## Run and test

```powershell
dotnet restore
dotnet test
dotnet run --project src/SafeVault.Api.csproj
```

The repository includes unit tests for SQL-injection payloads, XSS payloads, invalid email addresses, JWT validation, and role enforcement. `SECURITY_REPORT.md` summarizes the vulnerabilities, fixes, and the way Microsoft Copilot was used and reviewed.
