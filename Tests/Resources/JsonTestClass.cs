using System.Diagnostics.CodeAnalysis;

namespace Tests.Resources;

[ExcludeFromCodeCoverage]
internal class JsonTestClass
{
  public List<BaseClass> ListExample { get; set; } = new List<BaseClass>();
  public int IntExample { get; set; } = 0;
  public string StringExample { get; set; } = "";

  public override bool Equals(object? obj)
  {
    var testObj = obj as JsonTestClass;
    if (testObj == null) return false;
    return IntExample == testObj.IntExample && StringExample == testObj.StringExample;
  }
}
