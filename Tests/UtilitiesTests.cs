using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Tests.Resources;
using TNT.Utilities;

namespace Tests;

[ExcludeFromCodeCoverage]
public class UtilitiesTests
{
	[Fact]
	public void Utilities_GetAssemblyAttribute_Test()
	{
		Assembly asm = Assembly.LoadFrom(@"tests.dll");

		Assert.NotNull(asm);

		Attribute? attr = Utilities.GetAssemblyAttribute<AssemblyCompanyAttribute>(asm);
		Assert.Equal("Company", (attr as AssemblyCompanyAttribute)?.Company);

		attr = Utilities.GetAssemblyAttribute<AssemblyConfigurationAttribute>(asm);
		Assert.Equal("Debug", (attr as AssemblyConfigurationAttribute)?.Configuration);

		attr = Utilities.GetAssemblyAttribute<AssemblyCopyrightAttribute>(asm);
		Assert.Equal("Copyright", (attr as AssemblyCopyrightAttribute)?.Copyright);

		attr = Utilities.GetAssemblyAttribute<AssemblyDescriptionAttribute>(asm);
		Assert.Equal("Description", (attr as AssemblyDescriptionAttribute)?.Description);

		attr = Utilities.GetAssemblyAttribute<AssemblyFileVersionAttribute>(asm);
		Assert.Equal("3.3.3.3", (attr as AssemblyFileVersionAttribute)?.Version);

		//attr = Utilities.GetAssemblyAttribute<AssemblyInformationalVersionAttribute>(asm);
		//Assert.Equal("3.3.3.3", (attr as AssemblyInformationalVersionAttribute).InformationalVersion);

		attr = Utilities.GetAssemblyAttribute<AssemblyTitleAttribute>(asm);
		Assert.Equal("Tests", (attr as AssemblyTitleAttribute)?.Title);

		//attr = Utilities.GetAssemblyAttribute<AssemblyVersionAttribute>(asm);
		//Assert.Equal("3.3.3.3", (attr as AssemblyVersionAttribute).Version);

		attr = Utilities.GetAssemblyAttribute<AssemblyProductAttribute>(asm);
		Assert.Equal("Product", (attr as AssemblyProductAttribute)?.Product);

		//attr = Utilities.GetAssemblyAttribute<GuidAttribute>(asm);
		//Assert.Equal("eae4b166-b50f-4f09-aa59-23d18cfb4c5a", (attr as GuidAttribute).Value);
	}

	[Fact]
	public void Utilities_SerializeDeserialize_Tests()
	{
		RegistrationKey regKey = new RegistrationKey() { Authorization = "AuthorizationKey", License = "LicenseKey" };
		string fileName = Path.GetTempFileName();

		Utilities.SerializeToFile(regKey, fileName);
		RegistrationKey? regKey1 = Utilities.DeserializeFromFile<RegistrationKey>(fileName);

		Assert.Equal(regKey, regKey1);

		try
		{
			List<string>? strings = Utilities.DeserializeFromFile<List<string>>(fileName);
			throw new Exception();
		}
		catch (Exception ex)
		{
			Assert.IsType<InvalidOperationException>(ex);
		}

		File.Delete(fileName);

		try
		{
			regKey1 = Utilities.DeserializeFromFile<RegistrationKey>(fileName);
			Assert.True(false, "FileNotFoundException expected");
		}
		catch (Exception ex)
		{
			Assert.IsType<FileNotFoundException>(ex);
		}
	}

	[Fact]
	public void Utilities_GetTypes_Test()
	{
		string assemblyFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\TNT.Utilities.dll";

		var types = Utilities.GetTypes(assemblyFile, t => { return t.Namespace?.StartsWith("TNT.Utilities") ?? false; });

		Assert.Equal(15, types.Length);

		types = Utilities.GetTypes(assemblyFile, t =>
		{
			return t.IsVisible;
		});

		Assert.Equal(11, types.Length);
	}

	[Fact]
	public void Utilities_Serialize_Deserialize_Test()
	{
		var listClassPre = new ListClass();
		listClassPre.Add(new ExtendedClass1 { e1IntProperty = 1, e1StringProperty = "one", baseIntProperty = -1, baseStringProperty = "base1" });
		listClassPre.Add(new ExtendedClass2 { e2IntProperty = 2, e2StringProperty = "two", baseIntProperty = -2, baseStringProperty = "base2" });

		var types = new Type[] { typeof(ExtendedClass1), typeof(ExtendedClass2) };

		var str = Utilities.Serialize(listClassPre, types);

		var listClassPost = Utilities.Deserialize<ListClass>(str, types);

		var extClass1 = listClassPost[0] as ExtendedClass1;
		var extClass2 = listClassPost[1] as ExtendedClass2;

		Assert.NotNull(extClass1);
		Assert.NotNull(extClass2);

		Assert.Equal(1, extClass1.e1IntProperty);
		Assert.Equal("one", extClass1.e1StringProperty);
		Assert.Equal(-1, extClass1.baseIntProperty);
		Assert.Equal("base1", extClass1.baseStringProperty);

		Assert.Equal(2, extClass2.e2IntProperty);
		Assert.Equal("two", extClass2.e2StringProperty);
		Assert.Equal(-2, extClass2.baseIntProperty);
		Assert.Equal("base2", extClass2.baseStringProperty);
	}
}
