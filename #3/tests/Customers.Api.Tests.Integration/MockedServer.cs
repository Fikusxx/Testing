using WireMock.ResponseBuilders;
using WireMock.RequestBuilders;
using WireMock.Server;

namespace Customers.Api.Tests.Integration;

public class MockedServer : IDisposable
{
	private WireMockServer server;
	public string Url => server.Url!;


	public void Start()
	{
		server = WireMockServer.Start();
	}

	public void SetupUser(string username)
	{
		server.Given(Request.Create()
			.WithPath($"/example/{username}")
			.UsingGet())
			.RespondWith(Response.Create()
			.WithBodyAsJson(new { Message = "KEKW" })
			.WithHeader("content-type", "application/json; charset=utf-8")
			.WithStatusCode(200));
	}

	public void SetupThrottledUser(string username)
	{
		server.Given(Request.Create()
			.WithPath($"/example/{username}")
			.UsingGet())
			.RespondWith(Response.Create()
			.WithBodyAsJson(new { Message = "API Rate limit exceeded" })
			.WithHeader("content-type", "application/json; charset=utf-8")
			.WithStatusCode(403));
	}

	public void Dispose()
	{
		server.Stop();
		server.Dispose();
	}
}
