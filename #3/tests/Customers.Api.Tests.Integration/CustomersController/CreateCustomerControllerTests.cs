

namespace Customers.Api.Tests.Integration.CustomersController;

public class CreateCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
	private readonly HttpClient httpClient;
	private readonly Faker<User> userFaker = new Faker<User>()
		.RuleFor(x => x.Name, faker => faker.Person.FirstName)
		.RuleFor(x => x.Email, faker => faker.Person.Email)
		.RuleFor(x => x.Age, 25)
		.RuleFor(x => x.DOB, faker => faker.Person.DateOfBirth);

	public CreateCustomerControllerTests(CustomerApiFactory factory)
	{
		httpClient = factory.CreateClient();
	}

	[Fact]
	public async Task Create_ReturnsOk_Whenever()
	{
		var user = userFaker.Generate();

		//var user = userFaker.Clone()
		//	.RuleFor(x => x.Email, "notemail")
		//	.Generate();

		var response = await httpClient.PostAsJsonAsync("customers", user);
		var customerResponse = await response.Content.ReadFromJsonAsync<User>();

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		customerResponse.Should().BeEquivalentTo(user);
	}

	[Fact]
	public async Task Create_ReturnsInternalServerError_WhenRequestIsThrottled()
	{
		// Arrange 
		var user = userFaker.Clone()
			.RuleFor(x => x.Email, "throttledUserName")
			.Generate();

		// Act
		var response = await httpClient.PostAsJsonAsync("customers", user);

		// Asserty
		response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
	}
}

public class User
{
	public string Name { get; set; }
	public string Email { get; set; }
	public int Age { get; set; }
	public DateTimeOffset DOB { get; set; }
}
