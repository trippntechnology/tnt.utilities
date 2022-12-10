using System.Diagnostics.CodeAnalysis;

namespace Tests.Resources
{
	[ExcludeFromCodeCoverage]
	public class BaseClass
	{
		public int baseIntProperty { get; set; }

		public string? baseStringProperty { get; set; }
	}
}
