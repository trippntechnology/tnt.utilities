using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using TNT.Utilities;
using TNT.Utilities.CustomAttributes;

namespace Tests;

[ExcludeFromCodeCoverage]
public class ReflectorTests
{
  [Test]
  public void Test1()
  {
    TestClass tc = new TestClass() { EnumProperty = Enum1.Value2, IntProperty = 41, NoAttributes = "None", StringProperty = "Forty-one" };
    Reflector<TestClass> reflector = new Reflector<TestClass>(tc);

    Assert.That(tc.EnumProperty, Is.EqualTo(Enum1.Value2));
    Assert.That(tc.IntProperty, Is.EqualTo(41));
    Assert.That(tc.NoAttributes, Is.EqualTo("None"));
    Assert.That(tc.StringProperty, Is.EqualTo("Forty-one"));

    var props = (from p in reflector.Properties orderby p.Category, p.DisplayName select p).ToList();

    Assert.That(props.Count(), Is.EqualTo(4));
    Assert.That(string.IsNullOrEmpty(props[0].Category), Is.True);
    Assert.That(props[1].Category, Is.EqualTo("Cat1"));
    Assert.That(props[2].Category, Is.EqualTo("Cat1"));
    Assert.That(props[3].Category, Is.EqualTo("Cat2"));

    Assert.That(props[0].DisplayName, Is.EqualTo("NoAttributes"));
    Assert.That(props[1].DisplayName, Is.EqualTo("Int Property"));
    Assert.That(props[2].DisplayName, Is.EqualTo("String Property"));
    Assert.That(props[3].DisplayName, Is.EqualTo("Enumeration Property"));

    Assert.That(props[0].Value, Is.EqualTo("None"));
    Assert.That(props[1].Value, Is.EqualTo(41));
    Assert.That(props[2].Value, Is.EqualTo("Forty-one"));
    Assert.That(props[3].Value, Is.EqualTo(Enum1.Value2));

    props = (from p in reflector.Properties orderby p.Priority select p).ToList();

    Assert.That(props[0].Category, Is.EqualTo("Cat2"));
    Assert.That(props[1].Category, Is.EqualTo("Cat1"));
    Assert.That(string.IsNullOrEmpty(props[2].Category), Is.True);
    Assert.That(props[3].Category, Is.EqualTo("Cat1"));

    List<string> orderedCats = reflector.GetCategoriesByPriority();

    Assert.That(orderedCats.Count(), Is.EqualTo(3));
    Assert.That(orderedCats[0], Is.EqualTo("Cat2"));
    Assert.That(orderedCats[1], Is.EqualTo("Cat1"));
    Assert.That(orderedCats[2], Is.EqualTo(""));
  }

  [Test]
  public void Null_Constructor_Parameter()
  {
    Object? obj = null;
    Reflector<object?> reflector = new Reflector<Object?>(obj);
    Assert.That(reflector.Properties.Count, Is.EqualTo(0));
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
  [System.ComponentModel.Category("Cat1")]
  [DisplayName("Int Property")]
  public int IntProperty { get; set; }

  [PropertyReflector()]
  [System.ComponentModel.Category("Cat2")]
  [DisplayName("Enumeration Property")]
  public Enum1 EnumProperty { get; set; }

  [PropertyReflector(Priority = 2)]
  public string? NoAttributes { get; set; }

  [PropertyReflector(Priority = 1)]
  [System.ComponentModel.Category("Cat1")]
  [DisplayName("String Property")]
  public string? StringProperty { get; set; }

  public int IntNoReflect { get; set; }

  public string? StringNoReflect { get; set; }
}
