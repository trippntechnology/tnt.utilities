using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using TNT.Utilities;
using TNT.Utilities.CustomAttributes;

namespace Tests;

[ExcludeFromCodeCoverage]
public class ReflectorTests
{
	[Fact]
	public void Test1()
	{
		TestClass tc = new TestClass() { EnumProperty = Enum1.Value2, IntProperty = 41, NoAttributes = "None", StringProperty = "Forty-one" };
		Reflector<TestClass> reflector = new Reflector<TestClass>(tc);

		Assert.Equal(Enum1.Value2, tc.EnumProperty);
		Assert.Equal(41, tc.IntProperty);
		Assert.Equal("None", tc.NoAttributes);
		Assert.Equal("Forty-one", tc.StringProperty);

		var props = (from p in reflector.Properties orderby p.Category, p.DisplayName select p).ToList();

		Assert.Equal(4, props.Count());
		Assert.True(string.IsNullOrEmpty(props[0].Category));
		Assert.Equal("Cat1", props[1].Category);
		Assert.Equal("Cat1", props[2].Category);
		Assert.Equal("Cat2", props[3].Category);

		Assert.Equal("NoAttributes", props[0].DisplayName);
		Assert.Equal("Int Property", props[1].DisplayName);
		Assert.Equal("String Property", props[2].DisplayName);
		Assert.Equal("Enumeration Property", props[3].DisplayName);

		Assert.Equal("None", props[0].Value);
		Assert.Equal(41, props[1].Value);
		Assert.Equal("Forty-one", props[2].Value);
		Assert.Equal(Enum1.Value2, props[3].Value);

		props = (from p in reflector.Properties orderby p.Priority select p).ToList();

		Assert.Equal("Cat2", props[0].Category);
		Assert.Equal("Cat1", props[1].Category);
		Assert.True(string.IsNullOrEmpty(props[2].Category));
		Assert.Equal("Cat1", props[3].Category);

		List<string> orderedCats = reflector.GetCategoriesByPriority();

		Assert.Equal(3, orderedCats.Count());
		Assert.Equal("Cat2", orderedCats[0]);
		Assert.Equal("Cat1", orderedCats[1]);
		Assert.Equal("", orderedCats[2]);
	}
}

public enum Enum1
{
	Value1,
	Value2,
	Value3
}

[ExcludeFromCodeCoverage]
public class TestClass
{
	[PropertyReflector(Priority = 11)]
	[Category("Cat1")]
	[DisplayName("Int Property")]
	public int IntProperty { get; set; }

	[PropertyReflector()]
	[Category("Cat2")]
	[DisplayName("Enumeration Property")]
	public Enum1 EnumProperty { get; set; }

	[PropertyReflector(Priority = 2)]
	public string NoAttributes { get; set; }

	[PropertyReflector(Priority = 1)]
	[Category("Cat1")]
	[DisplayName("String Property")]
	public string StringProperty { get; set; }

	public int IntNoReflect { get; set; }

	public string StringNoReflect { get; set; }
}
