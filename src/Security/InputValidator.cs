using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace SafeVault.Security;

public static partial class InputValidator
{
    [GeneratedRegex("^[A-Za-z0-9_.-]{3,50}$", RegexOptions.CultureInvariant)]
    private static partial Regex UsernamePattern();

    public static bool IsSafeUsername(string? value) =>
        value is not null && UsernamePattern().IsMatch(value);

    public static bool IsValidEmail(string? value) =>
        value is not null && new EmailAddressAttribute().IsValid(value) && value.Length <= 254;

    public static string EncodeForHtml(string value) => HtmlEncoder.Default.Encode(value);
}
