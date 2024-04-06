using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Tests.Resources;
using TNT.Utilities;

namespace Tests;

[ExcludeFromCodeCoverage]
public class UtilitiesTests
{
  [Test]
  public void Utilities_GetAssemblyAttribute_Test()
  {
    Assembly asm = Assembly.LoadFrom(@"tests.dll");

    Assert.That(asm, Is.Not.Null);

    Attribute? attr = Utilities.GetAssemblyAttribute<AssemblyCompanyAttribute>(asm);
    Assert.That((attr as AssemblyCompanyAttribute)?.Company, Is.EqualTo("Company"));

    attr = Utilities.GetAssemblyAttribute<AssemblyConfigurationAttribute>(asm);
    Assert.That((attr as AssemblyConfigurationAttribute)?.Configuration, Is.EqualTo("Debug"));

    attr = Utilities.GetAssemblyAttribute<AssemblyCopyrightAttribute>(asm);
    Assert.That((attr as AssemblyCopyrightAttribute)?.Copyright, Is.EqualTo("Copyright"));

    attr = Utilities.GetAssemblyAttribute<AssemblyDescriptionAttribute>(asm);
    Assert.That((attr as AssemblyDescriptionAttribute)?.Description, Is.EqualTo("Description"));

    attr = Utilities.GetAssemblyAttribute<AssemblyFileVersionAttribute>(asm);
    Assert.That((attr as AssemblyFileVersionAttribute)?.Version, Is.EqualTo("3.3.3.3"));

    //attr = Utilities.GetAssemblyAttribute<AssemblyInformationalVersionAttribute>(asm);
    //Assert.AreEqual("3.3.3.3", (attr as AssemblyInformationalVersionAttribute).InformationalVersion);

    attr = Utilities.GetAssemblyAttribute<AssemblyTitleAttribute>(asm);
    Assert.That((attr as AssemblyTitleAttribute)?.Title, Is.EqualTo("Tests"));

    //attr = Utilities.GetAssemblyAttribute<AssemblyVersionAttribute>(asm);
    //Assert.AreEqual("3.3.3.3", (attr as AssemblyVersionAttribute).Version);

    attr = Utilities.GetAssemblyAttribute<AssemblyProductAttribute>(asm);
    Assert.That((attr as AssemblyProductAttribute)?.Product, Is.EqualTo("Product"));

    //attr = Utilities.GetAssemblyAttribute<GuidAttribute>(asm);
    //Assert.AreEqual("eae4b166-b50f-4f09-aa59-23d18cfb4c5a", (attr as GuidAttribute).Value);

    attr = Utilities.GetAssemblyAttribute<AssemblyCultureAttribute>(asm);
    Assert.That(attr, Is.Null);
  }

  [Test]
  public void Utilities_SerializeDeserialize_Tests()
  {
    RegistrationKey regKey = new RegistrationKey() { Authorization = "AuthorizationKey", License = "LicenseKey" };
    string fileName = Path.GetTempFileName();

    Utilities.SerializeToFile(regKey, fileName);
    RegistrationKey? regKey1 = Utilities.DeserializeFromFile<RegistrationKey>(fileName);

    Assert.That(regKey1, Is.EqualTo(regKey));

    try
    {
      List<string>? strings = Utilities.DeserializeFromFile<List<string>>(fileName);
      throw new Exception();
    }
    catch (Exception ex)
    {
      Assert.That(ex is InvalidOperationException, Is.True);
    }

    File.Delete(fileName);

    try
    {
      regKey1 = Utilities.DeserializeFromFile<RegistrationKey>(fileName);
      Assert.Fail("FileNotFoundException expected");
    }
    catch (Exception ex)
    {
      Assert.That(ex is FileNotFoundException, Is.True);
    }
  }

  [Test]
  public void Utilities_GetTypes_Test1()
  {
    var exAss = Assembly.GetExecutingAssembly();
    Assert.That(exAss, Is.Not.Null);

    var types = Utilities.GetTypes(exAss, t => t.Namespace?.StartsWith("Tests.Resources") ?? false);
    Assert.That(types?.Length, Is.EqualTo(6));
  }

  [Test]
  [Obsolete]
  public void Utilities_GetTypes_Test2()
  {
    string utilitiesAssemblyFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\TNT.Utilities.dll";
    var types = Utilities.GetTypes(utilitiesAssemblyFile, t => true);
    Assert.That(types?.Length, Is.EqualTo(16));
  }

  [Test]
  public void Utilities_Serialize_Deserialize_Test()
  {
    var listClassPre = new ListClass();
    listClassPre.Add(new ExtBaseClass1 { e1IntProperty = 1, e1StringProperty = "one", baseIntProperty = -1, baseStringProperty = "base1" });
    listClassPre.Add(new ExtBaseClass2 { e2IntProperty = 2, e2StringProperty = "two", baseIntProperty = -2, baseStringProperty = "base2" });

    var types = Utilities.GetTypes(Assembly.GetExecutingAssembly(), t => t.InheritsFrom(typeof(BaseClass)));
    var str = Utilities.Serialize(listClassPre, types);

    var listClassPost = Utilities.Deserialize<ListClass>(str, types) ?? new ListClass();

    var extClass1 = listClassPost[0] as ExtBaseClass1;
    var extClass2 = listClassPost[1] as ExtBaseClass2;

    Assert.That(extClass1, Is.Not.Null);
    Assert.That(extClass2, Is.Not.Null);

    Assert.That(extClass1.e1IntProperty, Is.EqualTo(1));
    Assert.That(extClass1.e1StringProperty, Is.EqualTo("one"));
    Assert.That(extClass1.baseIntProperty, Is.EqualTo(-1));
    Assert.That(extClass1.baseStringProperty, Is.EqualTo("base1"));

    Assert.That(extClass2.e2IntProperty, Is.EqualTo(2));
    Assert.That(extClass2.e2StringProperty, Is.EqualTo("two"));
    Assert.That(extClass2.baseIntProperty, Is.EqualTo(-2));
    Assert.That(extClass2.baseStringProperty, Is.EqualTo("base2"));
  }

  [Test]
  public void Utilities_InheritsFrom_Test()
  {
    var baseClassType = typeof(BaseClass);
    var extClass1Type = typeof(ExtBaseClass1);
    var extExtClass1Type = typeof(ExtExtBaseClass1);

    Assert.That(extClass1Type.InheritsFrom(baseClassType), Is.True);
    Assert.That(extClass1Type.InheritsFrom(null), Is.True);
    Assert.That(baseClassType.InheritsFrom(baseClassType), Is.True);
    Assert.That(extExtClass1Type.InheritsFrom(baseClassType), Is.True);
    Assert.That(typeof(Object).InheritsFrom(baseClassType), Is.False);
  }

  [Test]
  public void Utilities_To_From_ByteArray_Test()
  {
    var sut = new BaseClass() { baseIntProperty = 7, baseStringProperty = "seven" };
    byte[] sutBytes = Utilities.ToByteArray(sut);
    var newSut = Utilities.FromByteArray<BaseClass>(sutBytes);
    Assert.That(newSut, Is.EqualTo(sut));
  }
}
