using System.Diagnostics.CodeAnalysis;

namespace Tests.Resources
{
	[ExcludeFromCodeCoverage]
	public class ExtendedClass2 : BaseClass
	{
		public int e2IntProperty { get; set; }

		public string? e2StringProperty { get; set; }
	}
}
