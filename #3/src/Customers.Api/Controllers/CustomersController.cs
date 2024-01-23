using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Controllers;

[ApiController]
[Route("customers")]
public class CustomersController : ControllerBase
{
	private readonly IHttpClientFactory factory;

	public CustomersController(IHttpClientFactory factory)
	{
		this.factory = factory;
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var client = factory.CreateClient("Mocked");
		var response = await client.GetAsync("/example");
		var result = await response.Content.ReadAsStringAsync();

		return Ok(result);
		//return Ok(new { Message = "Privet" });
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] User user)
	{
		return Ok(user);
	}
}

public class User
{
	public string Name { get; set; }
	public string Email { get; set; }
	public int Age { get; set; }
	public DateTimeOffset DOB { get; set; }
}
