using System.Diagnostics.CodeAnalysis;

namespace Tests.Resources;

[ExcludeFromCodeCoverage]
public class BaseClass
{
  public int baseIntProperty { get; set; } = 0;

  public string? baseStringProperty { get; set; } = null;

  public override bool Equals(object? obj)
  {
    var other = obj as BaseClass;
    if (other == null) return false;
    return other.baseIntProperty == baseIntProperty && other.baseStringProperty == baseStringProperty;
  }

  public override int GetHashCode()
  {
    unchecked // Allow arithmetic overflow, just wrap around
    {
      int hash = 17;
      hash = hash * 23 + baseIntProperty.GetHashCode();
      hash = hash * 23 + (baseStringProperty?.GetHashCode() ?? 0);
      return hash;
    }
  }
}
