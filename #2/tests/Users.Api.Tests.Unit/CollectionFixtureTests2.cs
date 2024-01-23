using Xunit.Abstractions;

namespace Users.Api.Tests.Unit;

[Collection("shared")]
public class CollectionFixtureTests2
{
	private readonly MyClassFixture fixture;
	private readonly ITestOutputHelper output;

	public CollectionFixtureTests2(MyClassFixture fixture, ITestOutputHelper output)
	{
		this.fixture = fixture;
		this.output = output;
	}

	[Fact]
	public void Test_1()
	{
		output.WriteLine("Fixture: " + fixture.Id.ToString()); // same
	}
}
