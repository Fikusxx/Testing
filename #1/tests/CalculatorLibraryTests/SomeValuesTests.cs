

namespace Library.Tests.Unit;

public class SomeValuesTests
{
	private readonly SomeValues sut = new();

	[Fact]
	public void StringAssertions()
	{
		var name = sut.Name;

		name.Should().Be("Fikus");

		name.Should().NotBeNullOrWhiteSpace();

		name.Should().StartWith("F");
		name.Should().EndWith("s");
	}

	[Fact]
	public void NumberAssertions()
	{
		var age = sut.Age;

		age.Should().Be(32);

		age.Should().BePositive();

		age.Should().BeGreaterThan(30);
		age.Should().BeGreaterThanOrEqualTo(32);
		age.Should().BeLessThan(33);
		age.Should().BeLessThanOrEqualTo(32);

		age.Should().BeInRange(10, 50);
	}

	[Fact]
	public void DatesAssertions()
	{
		var dob = sut.DOB;

		dob.Should().Be(new(new(1991, 5, 8)));

		dob.Should().HaveYear(1991);
		dob.Should().HaveMonth(5);
		dob.Should().HaveDay(8);
	}

	[Fact]
	public void ObjectAssertions()
	{
		var user = sut.User;

		var expected = new User() { Age = 32, Name = "Fikus", DOB = new(new(1991, 5, 8)) };

		user.Should().BeEquivalentTo(expected);

		user.Should().BeEquivalentTo(expected, options => options
										.Excluding(x => x.DOB)
										.Excluding(x => x.Age));

		user.Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
	}

	[Fact]
	public void EnumerablesAssertions()
	{
		var users = sut.Users.As<User[]>();

		var expected = new User() { Age = 15, Name = "Vasya", DOB = new(dateTime: new(1990, 1, 1)) };

		users.Should().ContainEquivalentOf(expected);

		users.Should().HaveCount(3);

		users.Should().Contain(x => x.Name.StartsWith("Va") && x.Age >= 15);
	}

	[Fact]
	public void InternalAssertions()
	{
		var number = sut.SecretNumber;

		number.Should().Be(777);
	}
}
