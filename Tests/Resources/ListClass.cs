using System.Diagnostics.CodeAnalysis;

namespace Tests.Resources
{
  [ExcludeFromCodeCoverage]
  public class ListClass : List<BaseClass>
  {
    public int MyProperty { get; set; } = 10;
  }
}
