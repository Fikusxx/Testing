using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Security.Claims;

namespace Players.Api.Tests.Integration.Auth;

public sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) 
		: base(options, logger, encoder)
	{ }

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		//var identity = new ClaimsIdentity(Array.Empty<Claim>(), "Test");
		var identity = new ClaimsIdentity(new List<Claim>() { new("MY_TEST_CLAIM", "TEST_VALUE")}, "Test");
		var principal = new ClaimsPrincipal(identity);
		var ticket = new AuthenticationTicket(principal, "TestScheme");

		var result = AuthenticateResult.Success(ticket);

		return Task.FromResult(result);
	}
}
