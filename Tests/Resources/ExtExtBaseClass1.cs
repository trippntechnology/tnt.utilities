using System.Diagnostics.CodeAnalysis;

namespace Tests.Resources;

	[ExcludeFromCodeCoverage]
	public class ExtExtBaseClass1 : ExtBaseClass1
	{
		public long MyLong { get; set; }

		public override bool Equals(object? obj)
		{
			var other = obj as ExtExtBaseClass1;
			if (other == null) return false;
			return base.Equals(obj) && other.MyLong == MyLong;
		}

  public override int GetHashCode()
  {
    unchecked // Allow arithmetic overflow, just wrap around
    {
      int hash = base.GetHashCode();
      hash = hash * 23 + MyLong.GetHashCode();
      return hash;
    }
  }
}
