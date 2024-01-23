using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Players.Application;

public static class DI
{
	public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
	{
		services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

		return services;
	}
}


