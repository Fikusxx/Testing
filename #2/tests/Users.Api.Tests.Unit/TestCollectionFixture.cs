
namespace Users.Api.Tests.Unit;

[CollectionDefinition("shared")]
public class TestCollectionFixture : ICollectionFixture<MyClassFixture>
{ }

public class MyClassFixture
{
	public Guid Id { get; set; } = Guid.NewGuid();
}
