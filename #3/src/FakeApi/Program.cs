using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;


var server = WireMockServer.Start();

// Server is running on URL: http://localhost:62838
Console.WriteLine("Server is running on URL: " + server.Url);

server.Given(Request.Create()
					.WithPath("/example")
					.UsingGet())
	  .RespondWith(Response.Create()
					.WithBodyAsJson(new { Message = "KEKW" })
					//.WithBody(@"{""message"" : ""KEKW""}")
					.WithHeader("content-type", "application/json; charset=utf-8")
					.WithStatusCode(200));

Console.ReadKey();

server.Dispose();