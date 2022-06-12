using System.Diagnostics.CodeAnalysis;
using TNT.Utilities;

namespace Tests;

[ExcludeFromCodeCoverage]
public class ExtensionTests
{
	[Fact]
	public void Extensions_Let()
	{
		var result = new FooExtension() { Value = 7 }.Let(it =>
		{
			it.Value = 10;
			return 20;
		});

		Assert.Equal(20, result);
	}

	[Fact]
	public void Extensions_Also()
	{
		var result = new FooExtension() { Value = 7 }.Also(it =>
	 {
		 it.Value = 10;
	 });

		Assert.Equal(10, result?.Value);
	}

	[Fact]
	public void Extension_whenType()
	{
		FooExtension value = new BarExtension();
		var success = false;

		value.whenType<FooExtension, BarExtension>(d =>
		{
			success = true;
		});

		Assert.True(success, "whenType failed to cast to type");

		value.whenType<FooExtension, FormatException>(d =>
		{
			success = false;
		});

		Assert.True(success);
	}
}

[ExcludeFromCodeCoverage]
class FooExtension
{
	public int Value { get; set; }
}

[ExcludeFromCodeCoverage]
class BarExtension : FooExtension
{
	public BarExtension() : base()
	{
	}
}
