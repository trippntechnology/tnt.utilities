using System.Diagnostics.CodeAnalysis;

namespace Tests.Resources
{
	[ExcludeFromCodeCoverage]
	public class BaseClass
	{
		public int baseIntProperty { get; set; }

		public string? baseStringProperty { get; set; }

		public override bool Equals(object? obj)
		{
			var other = obj as BaseClass;
			if (other == null) return false;
			return other.baseIntProperty == baseIntProperty && other.baseStringProperty == baseStringProperty;
		}
	}
}
