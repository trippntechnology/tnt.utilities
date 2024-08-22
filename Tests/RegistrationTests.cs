using System.Diagnostics.CodeAnalysis;
using System.Management;
using TNT.Utilities;
using ManagementObjects = System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>>;

namespace Tests;

[ExcludeFromCodeCoverage]
public class RegistrationTests
{
  [Test]
  public void GenerateSHA1HashTest()
  {
    Assert.That(Registration.GenerateSHA1Hash("Lorem ipsum dolor sit amet"), Is.EqualTo("OPAPhzjiQdrqbzf29VroQU17Ahk="));
    Assert.That(Registration.GenerateSHA1Hash("Lorem ipsum dolor sit amet, erexit per te in deinde"), Is.EqualTo("wSvFipxMDuPkAVHAXTFTQOU16eY="));
    Assert.That(Registration.GenerateSHA1Hash("Lorem ipsum dolor sit amet, natura omnes Hellenicus dixisset alia gaudio hoc ait Cumque persequatur sic nec appellarer in fuerat"), Is.EqualTo("43wA7VnwV/LeKHX8ZJBSPXaheRc="));
  }

  [Test]
  public void ValidateKeyTest()
  {
    Assert.That(Registration.ValidateHash("Lorem ipsum dolor sit amet", "OPAPhzjiQdrqbzf29VroQU17Ahk="), Is.True);
    Assert.That(Registration.ValidateHash("Lorem ipsum dolor sit amet, erexit per te in deinde", "wSvFipxMDuPkAVHAXTFTQOU16eY="), Is.True);
    Assert.That(Registration.ValidateHash("Lorem ipsum dolor sit amet, natura omnes Hellenicus dixisset alia gaudio hoc ait Cumque persequatur sic nec appellarer in fuerat", "43wA7VnwV/LeKHX8ZJBSPXaheRc="), Is.True);

    Assert.That(Registration.ValidateHash("Bogus seed", "OPAPhzjiQdrqbzf29VroQU17Ahk="), Is.False);
  }

  //[Test]
  public void GetVolumeSerialNumberTest()
  {
    var serialNumber = Registration.GetVolumeSerialNumber();
    Assert.That(Registration.GetVolumeSerialNumber(), Is.EqualTo("62A1A1A5"));
  }

  //[Test]
  public void GetManagementObjectsTest()
  {
    try
    {
      Registration.GetManagementObjects("select from win32_logicaldisk");
    }
    catch (ManagementException me)
    {
      Assert.That(me.Message.Trim(), Is.EqualTo("Invalid query"));
    }
    catch (Exception)
    {
      Assert.That(false, Is.True, "Unexpected Exception");
    }

    ManagementObjects objs = Registration.GetManagementObjects("select * from win32_logicaldisk");

    Assert.That(objs, Is.Not.Null);
    Assert.That(objs.Count > 0, Is.True);

    Assert.That(objs[0]["Description"], Is.EqualTo("Local Fixed Disk"));
  }

  [Test]
  public void GenerateKeyTest()
  {
    string key = string.Empty;

    Assert.That(string.IsNullOrEmpty(Registration.GenerateKey(0, 0)), Is.True);

    Assert.That(Registration.GenerateKey(1, 0), Does.Match("^[A-Z]$"));
    Assert.That(Registration.GenerateKey(2, 0), Does.Match("^[A-Z]{2}$"));
    Assert.That(Registration.GenerateKey(2, 1), Does.Match("^[A-Z]-[A-Z]$"));
    Assert.That(Registration.GenerateKey(4, 1), Does.Match("^[A-Z](-[A-Z]){3}$"));
    Assert.That(Registration.GenerateKey(4, 0), Does.Match("^[A-Z]{4}$"));
    Assert.That(Registration.GenerateKey(4, 3), Does.Match("^[A-Z]{4}$"));
    Assert.That(Registration.GenerateKey(20, 4), Does.Match("^[A-Z]{4}(-[A-Z]{4}){4}$"));

    List<string> keys = new List<string>();

    for (int index = 0; index < 10000; index++)
    {
      key = Registration.GenerateKey(20, 4);

      if (keys.Contains(key))
      {
        Assert.Fail($"Failed at index {index} with {key}");
      }

      keys.Add(key);
    }
  }
}
