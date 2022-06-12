using System.Diagnostics.CodeAnalysis;
using System.Reflection;
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

		var types = Utilities.GetTypes(assemblyFile, t => { return true; });

		Assert.Equal(18, types.Length);

		types = Utilities.GetTypes(assemblyFile, t =>
		{
			return t.IsVisible;
		});

		Assert.Equal(11, types.Length);
	}
}
