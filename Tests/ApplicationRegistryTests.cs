using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows.Forms;
using TNT.Utilities;

namespace Tests;

[ExcludeFromCodeCoverage]
public class ApplicationRegistryTests
{
	const string COMPANY_NAME = "Tripp'n Technology";
	const string APP_NAME = "TNT.Utilities.Tests";

	public ApplicationRegistryTests()
	{
		string tntKey = string.Concat(@"SOFTWARE\", COMPANY_NAME);

		// Check if the appName key already exists
		RegistryKey tntRegKey = Registry.CurrentUser.CreateSubKey(tntKey);

		try
		{
			tntRegKey.DeleteSubKeyTree(APP_NAME);
		}
		catch { }
	}

	private ApplicationRegistry createAppReg(
		Form? form = null,
		RegistryKey? currentUser = null,
		string coName = COMPANY_NAME,
		string appName = APP_NAME)
	{
		currentUser = currentUser ?? Registry.CurrentUser;

		return form == null
			? new ApplicationRegistry(currentUser, coName, appName)
			: new ApplicationRegistry(form, currentUser, coName, appName);
	}

	[Fact]
	public void ReadWriteBoolean()
	{
		try
		{
			var appReg = createAppReg();
			string keyName = "BooleanTest";
			string subKeyName = "SubKey";

			Assert.True(appReg.ReadBoolean(keyName, true));

			appReg.WriteBoolean(keyName, true);
			Assert.True(appReg.ReadBoolean(keyName, false));

			appReg.WriteBoolean(keyName, false);
			Assert.False(appReg.ReadBoolean(keyName, true));

			Assert.True(appReg.ReadBoolean(keyName, keyName, true));

			appReg.WriteBoolean(subKeyName, keyName, true);
			Assert.True(appReg.ReadBoolean(subKeyName, keyName, false));

			appReg.WriteBoolean(subKeyName, keyName, false);
			Assert.False(appReg.ReadBoolean(subKeyName, keyName, true));

		}
		catch (Exception ex)
		{
			Assert.True(false, ex.Message);
		}
	}

	[Fact]
	public void ReadWriteInt()
	{
		try
		{
			var appReg = createAppReg();

			string keyName = "IntegerTest";
			string subKeyName = "SubKey";

			Assert.Equal(12, appReg.ReadInteger(keyName, 12));

			appReg.WriteInteger(keyName, 25);
			Assert.Equal(25, appReg.ReadInteger(keyName, 12));

			Assert.Equal(12, appReg.ReadInteger(subKeyName, keyName, 12));

			appReg.WriteInteger(subKeyName, keyName, 25);
			Assert.Equal(25, appReg.ReadInteger(subKeyName, keyName, 12));
		}
		catch (Exception ex)
		{
			Assert.True(false, ex.Message);
		}
	}

	[Fact]
	public void ReadWriteString()
	{
		try
		{
			var appReg = createAppReg();

			string keyName = "StringTest";
			string subKeyName = "SubKey";
			string value = "Test String";
			string defValue = "Default value";

			Assert.Equal(defValue, appReg.ReadString(keyName, defValue));

			appReg.WriteString(keyName, value);
			Assert.Equal(value, appReg.ReadString(keyName, defValue));

			Assert.Equal(defValue, appReg.ReadString(subKeyName, keyName, defValue));

			appReg.WriteString(subKeyName, keyName, value);
			Assert.Equal(value, appReg.ReadString(subKeyName, keyName, defValue));
		}
		catch (Exception ex)
		{
			Assert.True(false, ex.Message);
		}
	}

	[Fact]
	public void ReadWriteStringList()
	{
		try
		{
			var appReg = createAppReg();

			string keyName = "StringListTest";

			Assert.Equal(appReg.ReadStringList(keyName).Count, 0);

			List<string> expected = new List<string>(new string[] { "one", "two", "three", "four", "five" });

			appReg.WriteStringList(keyName, expected);

			Equals(expected, appReg.ReadStringList(keyName));
		}
		catch (Exception ex)
		{
			Assert.True(false, ex.Message);
		}
	}

	[Fact]
	public void ReadWriteIntList()
	{
		try
		{
			var appReg = createAppReg();

			string keyName = "IntegerListTest";
			Assert.Equal(appReg.ReadList<int>(keyName).Count, 0);

			List<int> expected = new List<int>(new int[] { -2, -1, 0, 1, 2 });

			appReg.WriteList(keyName, expected);

			Equals(expected, appReg.ReadList<int>(keyName));
		}
		catch (Exception ex)
		{
			Assert.True(false, ex.Message);
		}
	}

	[Fact]
	public void ReadWriteToolStripItems()
	{
		try
		{
			var appReg = createAppReg();

			string keyName = "ToolStripItemsTest";
			ToolStrip ts = new ToolStrip();
			ToolStripItemCollection tsic = new ToolStripItemCollection(ts, new ToolStripItem[0]);
			appReg.ReadToolStripItems(keyName, tsic);

			Assert.True(tsic == null || tsic.Count == 0);

			tsic = new ToolStripItemCollection(ts, new ToolStripItem[] { new ToolStripMenuItem("One"), new ToolStripMenuItem("Two"), new ToolStripMenuItem("Three") });

			appReg.WriteToolStripItems(keyName, tsic);

			ToolStripItemCollection newTSIC = new ToolStripItemCollection(ts, new ToolStripItem[0]);

			appReg.ReadToolStripItems(keyName, newTSIC);

			Assert.Equal(tsic.Count, newTSIC.Count);

			for (int index = 0; index < tsic.Count; index++)
			{
				Assert.Equal(tsic[index].Text, newTSIC[index].Text);
			}
		}
		catch (Exception ex)
		{
			Assert.True(false, ex.Message);
		}
	}

	[Fact]
	public void LoadSaveFormState()
	{
		try
		{
			var testForm1 = new Form();
			testForm1.Name = "TestForm";
			testForm1.Show();

			var appReg = createAppReg(testForm1);

			testForm1.Width = 777;
			testForm1.Height = 983;
			testForm1.Top = 34;
			testForm1.Left = 87;
			testForm1.WindowState = FormWindowState.Normal;

			testForm1.Close();

			Form testForm2 = new Form();
			testForm2.Name = "TestForm";
			appReg = createAppReg(testForm2);
			testForm2.Show();

			Assert.Equal(testForm1.Width, testForm2.Width);
			Assert.Equal(testForm1.Height, testForm2.Height);
			Assert.Equal(testForm1.Top, testForm2.Top);
			Assert.Equal(testForm1.Left, testForm2.Left);
			Assert.Equal(testForm1.WindowState, testForm2.WindowState);
		}
		catch (Exception ex)
		{
			Assert.True(false, ex.Message);
		}
	}

	[Fact]
	public void ReadWriteBytes()
	{
		try
		{
			var appReg = createAppReg();

			string keyName = "BinaryTest";

			Assert.Null(appReg.ReadBytes(keyName));

			byte[] bytes = Encoding.ASCII.GetBytes(keyName);
			appReg.WriteBytes(keyName, bytes);

			byte[]? readBytes = appReg.ReadBytes(keyName);

			Assert.NotNull(readBytes);

			string readString = Encoding.UTF8.GetString(readBytes!);

			Assert.Equal(keyName, readString);
		}
		catch (Exception ex)
		{
			Assert.True(false, ex.Message);
		}
	}

	[Fact]
	public void ReadWriteObject()
	{
		try
		{
			var appReg = createAppReg();

			string keyName = "ObjectTest";

			TestObject to = new TestObject()
			{
				intValue = 10,
				stringValue = "ten"
			};

			appReg.WriteObject(keyName, to);

			TestObject? newTO = appReg.ReadObject<TestObject>(keyName);

			Assert.Equal(to, newTO);
		}
		catch (Exception ex)
		{
			Assert.True(false, ex.Message);
		}
	}
}

[ExcludeFromCodeCoverage]
[Serializable]
public class TestObject
{
	public int intValue { get; set; }
	public string? stringValue { get; set; }

	public override bool Equals(object? obj)
	{
		TestObject? to = obj as TestObject;

		if (to == null)
		{
			return false;
		}

		return intValue == to.intValue && stringValue == to.stringValue;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}