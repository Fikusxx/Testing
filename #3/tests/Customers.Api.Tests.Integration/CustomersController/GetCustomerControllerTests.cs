
namespace Customers.Api.Tests.Integration.CustomersController;

public class GetCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
	private readonly HttpClient httpClient;
	private readonly Faker<User> userFaker = new Faker<User>()
		.RuleFor(x => x.Name, faker => faker.Person.FirstName)
		.RuleFor(x => x.Email, faker => faker.Person.Email)
		.RuleFor(x => x.Age, 25)
		.RuleFor(x => x.DOB, faker => faker.Person.DateOfBirth);

	public GetCustomerControllerTests(CustomerApiFactory factory)
	{
		httpClient = factory.CreateClient();
	}

	[Fact]
	public async Task Get_ReturnsOk_Whenever()
	{
		var response = await httpClient.GetAsync("customers");
		var result = await response.Content.ReadAsStringAsync();

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task Get_ReturnsOk_Whenever2()
	{
		var response = await httpClient.GetAsync("customers");

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task Delete_ReturnsOk_WhenUserExists()
	{
		// Arrange
		var user = userFaker.Generate();
		await httpClient.PostAsJsonAsync("customers", user);

		// Act
		var response = await httpClient.DeleteAsync($"customers/{user.Name}");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task Delete_ReturnsNotFound_WhenUserDoesntExist()
	{
		// Act
		var response = await httpClient.DeleteAsync($"customers/someName");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}

