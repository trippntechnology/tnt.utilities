﻿using System.Diagnostics.CodeAnalysis;

namespace Tests.Resources
{
	[ExcludeFromCodeCoverage]
	public class ExtBaseClass1 : BaseClass
	{
		public int e1IntProperty { get; set; }

		public string? e1StringProperty { get; set; }
	}
}