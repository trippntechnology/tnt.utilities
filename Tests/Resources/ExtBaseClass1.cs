using System.Diagnostics.CodeAnalysis;

namespace Tests.Resources;

[ExcludeFromCodeCoverage]
public class ExtBaseClass1 : BaseClass
{
  public int e1IntProperty { get; set; } = 0;

  public string? e1StringProperty { get; set; } = null;

  public override bool Equals(object? obj)
  {
    var other = obj as ExtBaseClass1;
    if (other == null) return false;
    return base.Equals(obj) && e1IntProperty == other.e1IntProperty && e1StringProperty == other.e1StringProperty;
  }

  public override int GetHashCode()
  {
    unchecked // Allow arithmetic overflow, just wrap around
    {
      int hash = base.GetHashCode();
      hash = hash * 23 + e1IntProperty.GetHashCode();
      hash = hash * 23 + (e1StringProperty?.GetHashCode() ?? 0);
      return hash;
    }
  }
}