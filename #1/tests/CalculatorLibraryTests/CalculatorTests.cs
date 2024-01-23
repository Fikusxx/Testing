using System.Collections;
using Xunit.Abstractions;

namespace Library.Tests.Unit;

public class CalculatorTests
{
	private readonly ITestOutputHelper helper;
	private readonly Calculator sut = new();

	public CalculatorTests(ITestOutputHelper helper)
	{
		this.helper = helper;
		this.helper.WriteLine("Hello KEKW");
	}

	[Theory]
	[ClassData(typeof(CalculatorAddTestData))]
	public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers(int number1, int number2, int expected)
	{
		// Act
		var result = sut.Add(number1, number2);

		// Assert
		result.Should().Be(expected);
	}

	[Theory]
	[InlineData(1, 1, 0)]
	[InlineData(2, 1, 1)]
	[InlineData(-1, -1, 0)]
	[InlineData(5, 10, -5)]
	public void Subtract_ShouldSubtractTwoNumbers_WhenTwoNumbersAreIntegeres(int number1, int number2, int expected)
	{
		// Act
		var result = sut.Subtract(number1, number2);

		// Assert
		Assert.Equal(expected, result);
	}

	[Theory]
	[InlineData(1, 1, 1)]
	[InlineData(10, 5, 2)]
	public void Divide_ShouldDivide_WhenTwoNumbersAreIntegeres(int number1, int number2, int expected)
	{
		// Act 
		var result = sut.Divide(number1, number2);

		// Assert
		result.Should().Be(expected);
	}

	[Theory]
	[InlineData(5, 0, 0)]
	[InlineData(6, 0, 0)]
	[InlineData(777, 0, 0)]
	public void Divide_ShouldThrow_WhenDividerIsZero(int number1, int number2, int expected)
	{
		// Act
		Action result = () => sut.Divide(number1, number2);

		// Assert
		result.Should().Throw<Exception>();
	}
}

public class CalculatorAddTestData : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[] { 0, 0, 0 };
		yield return new object[] { 1, 1, 2 };
		yield return new object[] { -1, -1, -2 };
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}