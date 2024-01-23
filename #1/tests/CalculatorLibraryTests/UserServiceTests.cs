

namespace Library.Tests.Unit;

public class UserServiceTests
{
	private readonly UserService sut;
	private readonly IUserRepository ctx = Substitute.For<IUserRepository>();

	public UserServiceTests()
	{
		sut = new UserService(ctx);
	}

	[Fact]
	public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
	{
		// Arrage
		ctx.GetAllAsync().Returns(Array.Empty<User>());

		// Act
		var users = await sut.GetAllAsync();

		// Assert
		users.Should().BeEmpty();
	}

	[Fact]
	public async Task GetAllAsync_ShouldReturnListOfUsers_WhenUsersExist()
	{
		// Arrage
		var expected = new[]
		{
			new User(){Age = 1, Name = "Fikus", DOB = DateTimeOffset.UtcNow},
		};

		ctx.GetAllAsync().Returns(expected);

		// Act
		var users = await sut.GetAllAsync();

		// Assert
		users.Should().HaveCount(1);
		users.Should().BeEquivalentTo(expected);
		users.Should().ContainSingle(x => x.Name == "Fikus");
	}
}
