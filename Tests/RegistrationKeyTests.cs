using System.Diagnostics.CodeAnalysis;
using TNT.Utilities;

namespace Tests;

[ExcludeFromCodeCoverage]
internal class RegistrationKeyTests
{
	[Test]
	public void Equals_Test()
	{
		var regKey1 = new RegistrationKey { Authorization = "Authorization", License = "License" };
		var regKey2 = new RegistrationKey { Authorization = "Authorization", License = "License" };
		var regKey3 = new RegistrationKey { Authorization = "Auth", License = "Lic" };
		var obj = new Object();
		Assert.That(regKey1?.Equals(regKey2), Is.True);
		Assert.That(regKey1?.Equals(regKey3), Is.False);
		Assert.That(regKey1?.Equals(null), Is.False);
		Assert.That(regKey1?.Equals(obj), Is.False);
	}
}
