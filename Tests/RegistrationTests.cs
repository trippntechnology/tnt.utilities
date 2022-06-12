using System.Diagnostics.CodeAnalysis;
using System.Management;
using TNT.Utilities;
using ManagementObjects = System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>>;

namespace Tests;

[ExcludeFromCodeCoverage]
public class RegistrationTests
{
	[Fact]
	public void GenerateSHA1HashTest()
	{
		Assert.Equal("OPAPhzjiQdrqbzf29VroQU17Ahk=", Registration.GenerateSHA1Hash("Lorem ipsum dolor sit amet"));
		Assert.Equal("wSvFipxMDuPkAVHAXTFTQOU16eY=", Registration.GenerateSHA1Hash("Lorem ipsum dolor sit amet, erexit per te in deinde"));
		Assert.Equal("43wA7VnwV/LeKHX8ZJBSPXaheRc=", Registration.GenerateSHA1Hash("Lorem ipsum dolor sit amet, natura omnes Hellenicus dixisset alia gaudio hoc ait Cumque persequatur sic nec appellarer in fuerat"));
	}

	[Fact]
	public void ValidateKeyTest()
	{
		Assert.True(Registration.ValidateHash("Lorem ipsum dolor sit amet", "OPAPhzjiQdrqbzf29VroQU17Ahk="));
		Assert.True(Registration.ValidateHash("Lorem ipsum dolor sit amet, erexit per te in deinde", "wSvFipxMDuPkAVHAXTFTQOU16eY="));
		Assert.True(Registration.ValidateHash("Lorem ipsum dolor sit amet, natura omnes Hellenicus dixisset alia gaudio hoc ait Cumque persequatur sic nec appellarer in fuerat", "43wA7VnwV/LeKHX8ZJBSPXaheRc="));

		Assert.False(Registration.ValidateHash("Bogus seed", "OPAPhzjiQdrqbzf29VroQU17Ahk="));
	}

	[Fact]
	public void GetVolumeSerialNumberTest()
	{
		var serialNumber = Registration.GetVolumeSerialNumber();
		Assert.Equal("62A1A1A5", Registration.GetVolumeSerialNumber());
	}

	[Fact]
	public void GetManagementObjectsTest()
	{
		try
		{
			Registration.GetManagementObjects("select from win32_logicaldisk");
		}
		catch (ManagementException me)
		{
			Assert.Equal("Invalid query", me.Message.Trim());
		}
		catch (Exception)
		{
			Assert.True(false, "Unexpected Exception");
		}

		ManagementObjects objs = Registration.GetManagementObjects("select * from win32_logicaldisk");

		Assert.NotNull(objs);
		Assert.True(objs.Count > 0);

		Assert.Equal("Local Fixed Disk", objs[0]["Description"]);
	}

	[Fact]
	public void GenerateKeyTest()
	{
		string key = string.Empty;

		Assert.True(string.IsNullOrEmpty(Registration.GenerateKey(0, 0)));

		Assert.Matches("^[A-Z]$", Registration.GenerateKey(1, 0));
		Assert.Matches("^[A-Z]{2}$", Registration.GenerateKey(2, 0));
		Assert.Matches("^[A-Z]-[A-Z]$", Registration.GenerateKey(2, 1));
		Assert.Matches("^[A-Z](-[A-Z]){3}$", Registration.GenerateKey(4, 1));
		Assert.Matches("^[A-Z]{4}$", Registration.GenerateKey(4, 0));
		Assert.Matches("^[A-Z]{4}$", Registration.GenerateKey(4, 3));

		Assert.Matches("^[A-Z]{4}(-[A-Z]{4}){4}$", Registration.GenerateKey(20, 4));

		List<string> keys = new List<string>();

		for (int index = 0; index < 10000; index++)
		{
			key = Registration.GenerateKey(20, 4);

			if (keys.Contains(key))
			{
				Assert.True(false, $"Failed at index {index} with {key}");
			}

			keys.Add(key);
		}
	}
}
