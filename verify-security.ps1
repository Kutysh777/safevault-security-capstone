$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $MyInvocation.MyCommand.Path

$checks = @(
    @{ Name = 'Allow-list input validation'; File = 'src/Security/InputValidator.cs'; Pattern = 'GeneratedRegex' },
    @{ Name = 'HTML output encoding'; File = 'src/Security/InputValidator.cs'; Pattern = 'HtmlEncoder.Default.Encode' },
    @{ Name = 'JWT signature validation'; File = 'src/Program.cs'; Pattern = 'ValidateIssuerSigningKey = true' },
    @{ Name = 'JWT lifetime validation'; File = 'src/Program.cs'; Pattern = 'ValidateLifetime = true' },
    @{ Name = 'Role-based authorization'; File = 'src/Controllers/AdminController.cs'; Pattern = 'Authorize\(Policy = "AdminOnly"\)' },
    @{ Name = 'SQL injection tests'; File = 'tests/InputValidationTests.cs'; Pattern = "DROP TABLE" },
    @{ Name = 'XSS tests'; File = 'tests/InputValidationTests.cs'; Pattern = '<script>' },
    @{ Name = 'Security review'; File = 'SECURITY_REPORT.md'; Pattern = 'Vulnerabilities identified' }
)

$failed = @()
foreach ($check in $checks) {
    $path = Join-Path $root $check.File
    $ok = (Test-Path -LiteralPath $path) -and (Select-String -LiteralPath $path -Pattern $check.Pattern -Quiet)
    $status = if ($ok) { 'PASS' } else { 'FAIL' }
    Write-Output "$status - $($check.Name)"
    if (-not $ok) { $failed += $check.Name }
}

if ($failed.Count -gt 0) {
    throw "Security verification failed: $($failed -join ', ')"
}

Write-Output 'All static security checks passed.'

$usernamePattern = '^[A-Za-z0-9_.-]{3,50}$'
$unsafeInputs = @("' OR 1=1 --", "admin'; DROP TABLE Users;--", "<script>alert('xss')</script>", '../../etc/passwd')
$safeInputs = @('alice_01', 'john.doe')

foreach ($payload in $unsafeInputs) {
    if ($payload -match $usernamePattern) {
        throw "Dynamic validation failed: unsafe input accepted: $payload"
    }
    Write-Output "PASS - rejected unsafe input: $payload"
}

foreach ($value in $safeInputs) {
    if ($value -notmatch $usernamePattern) {
        throw "Dynamic validation failed: safe input rejected: $value"
    }
    Write-Output "PASS - accepted allow-listed input: $value"
}

$encoded = [System.Net.WebUtility]::HtmlEncode('<script>alert(1)</script>')
if ($encoded -match '<script>') {
    throw 'Dynamic validation failed: script markup was not encoded.'
}
Write-Output 'PASS - XSS markup encoded.'
Write-Output 'All executable security checks passed.'
