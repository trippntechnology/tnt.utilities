using System;

namespace TNT.Utilities.CustomAttributes
{
	/// <summary>
	/// Property attribute that indicates a property should be reflected by the Reflector
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class PropertyReflectorAttribute : Attribute
	{
		/// <summary>
		/// Indicates the priority. A lower value has a higher priority. Default is 0.
		/// </summary>
		public int Priority { get; set; }
	}
}
