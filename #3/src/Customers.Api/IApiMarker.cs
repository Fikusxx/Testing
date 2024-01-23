namespace Customers.Api;

public interface IApiMarker
{ }


public static class Extensions
{
	public static IServiceCollection AddSomething(this IServiceCollection services)
	{
		return services;
	}
}