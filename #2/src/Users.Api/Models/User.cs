namespace Users.Api.Models;

public class User
{
	public string Name { get; set; } = default!;
	public int Age { get; set; }
	public DateOnly DOB { get; set; }
}
