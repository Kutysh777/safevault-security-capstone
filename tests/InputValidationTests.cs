using SafeVault.Security;

namespace SafeVault.Tests;

public sealed class InputValidationTests
{
    [Theory]
    [InlineData("' OR 1=1 --")]
    [InlineData("admin'; DROP TABLE Users;--")]
    [InlineData("<script>alert('xss')</script>")]
    [InlineData("../../etc/passwd")]
    public void UsernameRejectsInjectionAndXss(string payload) =>
        Assert.False(InputValidator.IsSafeUsername(payload));

    [Theory]
    [InlineData("alice_01")]
    [InlineData("john.doe")]
    public void UsernameAcceptsAllowListedValues(string value) =>
        Assert.True(InputValidator.IsSafeUsername(value));

    [Fact]
    public void HtmlEncodingNeutralizesScriptMarkup()
    {
        var encoded = InputValidator.EncodeForHtml("<script>alert(1)</script>");
        Assert.DoesNotContain("<script>", encoded);
        Assert.Contains("&lt;script&gt;", encoded);
    }

    [Theory]
    [InlineData("user@example.com", true)]
    [InlineData("not-an-email", false)]
    public void EmailValidationIsStrict(string value, bool expected) =>
        Assert.Equal(expected, InputValidator.IsValidEmail(value));
}
