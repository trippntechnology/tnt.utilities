using System.Diagnostics.CodeAnalysis;

namespace Tests.Resources
{
	[ExcludeFromCodeCoverage]
	public class ExtBaseClass1 : BaseClass
	{
		public int e1IntProperty { get; set; }

		public string? e1StringProperty { get; set; }

		public override bool Equals(object? obj)
		{
			var other = obj as ExtBaseClass1;
			if (other == null) return false;
			return base.Equals(obj) && e1IntProperty == other.e1IntProperty && e1StringProperty == other.e1StringProperty;
		}
	}
}
