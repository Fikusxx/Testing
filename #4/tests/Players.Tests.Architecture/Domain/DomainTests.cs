using System.Text.Json.Serialization;

namespace Players.Tests.Architecture.Domain;

public sealed class DomainTests
{
	private static readonly Assembly domainAssembly = typeof(IMarkerAssembly).Assembly;

	[Fact]
	public void DomainEvents_ShouldBeSealed()
	{
		var result = Types.InAssembly(domainAssembly)
							.That()
							.ImplementInterface(typeof(IDomainEvent))
							.Should()
							.BeSealed()
							.GetResult();

		result.FailingTypes.Should().BeNullOrEmpty();
		result.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void DomainEvents_ShouldEndWithEvent()
	{
		var result = Types.InAssembly(domainAssembly)
			.That()
			.ImplementInterface(typeof(IDomainEvent))
			.Should()
			.HaveNameEndingWith("event")
			.GetResult();

		result.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void Entities_ShouldHaveInternalConstructorMarkedWithJsonConstructorAttribute()
	{
		var arTypes = Types.InAssembly(domainAssembly)
			.That()
			.Inherit(typeof(AggregateRoot))
			.GetTypes();

		var failingTypes = new List<Type>();

		foreach (var type in arTypes)
		{
			var ctors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
			var hasJsonCtorAttribute = ctors.Any(x => x.GetCustomAttribute(typeof(JsonConstructorAttribute)) is not null);

			if (hasJsonCtorAttribute == false)
			{
				failingTypes.Add(type);
			}
		}

		failingTypes.Should().BeEmpty();
	}

	[Fact]
	public void Domain_ShouldNotHaveDependencyOnApplication()
	{
		var result = Types.InAssembly(domainAssembly)
							.Should()
							.NotHaveDependencyOn("Application")
							.GetResult();

		result.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void Entities_ShouldNotHavePublicSetters()
	{
		var arTypes = Types.InAssembly(domainAssembly)
			.That()
			.Inherit(typeof(AggregateRoot))
			.GetTypes();

		var failingTypes = new List<Type>();

		foreach (var type in arTypes)
		{
			var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

			props.ForEach(x =>
			{
				if (x.GetSetMethod()?.IsPublic == true)
				{
					failingTypes.Add(type);
					return;
				}
			});
		}

		failingTypes.Should().BeEmpty();
	}
}
