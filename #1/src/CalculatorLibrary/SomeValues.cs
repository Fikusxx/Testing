
namespace Library;

public class SomeValues
{
	public string Name { get; set; } = "Fikus";
	public int Age { get; set; } = 32;
	public DateTimeOffset DOB { get; set; } = new(new(1991, 5, 8));
	public User User { get; set; } = new User() { Age = 32, Name = "Fikus", DOB = new(new(1991, 5, 8)) };
	public IEnumerable<User> Users { get; set; } = new[] {
		new User() { Age = 15, Name = "Vasya", DOB = new(new(1990, 1, 1)) },
		new User() { Age = 25, Name = "Petya", DOB = DateTimeOffset.UtcNow.AddYears(-25) },
		new User() { Age = 35, Name = "Vova", DOB = DateTimeOffset.UtcNow.AddYears(-35) },
	};
	internal int SecretNumber = 777;
	public event EventHandler ExampleEvent;
	public IEnumerable<int> Numbers = [1, 2, 3];

	internal void InternalMethod()
	{
		throw new Exception("GET OUTTA HERE");
	}
}

public class User
{
	public string Name { get; set; }
	public int Age { get; set; }
	public DateTimeOffset DOB { get; set; }
}
