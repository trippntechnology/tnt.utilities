
namespace TNT.Utilities
{
	/// <summary>
	/// Represents a registration key
	/// </summary>
	public class RegistrationKey
	{
		/// <summary>
		/// Authorization component
		/// </summary>
		public string? Authorization { get; set; }

		/// <summary>
		/// License component
		/// </summary>
		public string? License { get; set; }

		/// <summary>
		/// Determines if two <see cref="RegistrationKey"/> objects are equal
		/// </summary>
		/// <param name="obj">Object to compare</param>
		/// <returns>True if equal, false otherwise</returns>
		public override bool Equals(object? obj)
		{
			var regKey = obj as RegistrationKey;
			if (regKey == null) return false;

			return this.Authorization == regKey.Authorization && this.License == regKey.License;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current System.Object.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
