﻿using Microsoft.Win32;
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

  [Test]
  public void ReadWriteBoolean()
  {
    try
    {
      var appReg = createAppReg();
      string keyName = "BooleanTest";
      string subKeyName = "SubKey";

      Assert.That(appReg.ReadBoolean(keyName, true), Is.True);

      appReg.WriteBoolean(keyName, true);
      Assert.That(appReg.ReadBoolean(keyName, false), Is.True);

      appReg.WriteBoolean(keyName, false);
      Assert.That(appReg.ReadBoolean(keyName, true), Is.False);

      Assert.That(appReg.ReadBoolean(keyName, keyName, true), Is.True);

      appReg.WriteBoolean(subKeyName, keyName, true);
      Assert.That(appReg.ReadBoolean(subKeyName, keyName, false), Is.True);

      appReg.WriteBoolean(subKeyName, keyName, false);
      Assert.That(appReg.ReadBoolean(subKeyName, keyName, true), Is.False);

    }
    catch (Exception ex)
    {
      Assert.Fail(ex.Message);
    }
  }

  [Test]
  public void ReadWriteInt()
  {
    try
    {
      var appReg = createAppReg();

      string keyName = "IntegerTest";
      string subKeyName = "SubKey";

      Assert.That(appReg.ReadInteger(keyName, 12), Is.EqualTo(12));

      appReg.WriteInteger(keyName, 25);
      Assert.That(appReg.ReadInteger(keyName, 12), Is.EqualTo(25));

      Assert.That(appReg.ReadInteger(subKeyName, keyName, 12), Is.EqualTo(12));

      appReg.WriteInteger(subKeyName, keyName, 25);
      Assert.That(appReg.ReadInteger(subKeyName, keyName, 12), Is.EqualTo(25));
    }
    catch (Exception ex)
    {
      Assert.Fail(ex.Message);
    }
  }

  [Test]
  public void ReadWriteString()
  {
    try
    {
      var appReg = createAppReg();

      string keyName = "StringTest";
      string subKeyName = "SubKey";
      string value = "Test String";
      string defValue = "Default value";

      Assert.That(appReg.ReadString(keyName, defValue), Is.EqualTo(defValue));

      appReg.WriteString(keyName, value);
      Assert.That(appReg.ReadString(keyName, defValue), Is.EqualTo(value));

      Assert.That(appReg.ReadString(subKeyName, keyName, defValue), Is.EqualTo(defValue));

      appReg.WriteString(subKeyName, keyName, value);
      Assert.That(appReg.ReadString(subKeyName, "invalid_key", defValue), Is.EqualTo(defValue));
      Assert.That(appReg.ReadString(subKeyName, keyName, defValue), Is.EqualTo(value));
    }
    catch (Exception ex)
    {
      Assert.Fail(ex.Message);
    }
  }

  [Test]
  public void ReadWriteStringList()
  {
    try
    {
      var appReg = createAppReg();

      string keyName = "StringListTest";

      Assert.That(appReg.ReadStringList(keyName).Count, Is.EqualTo(0));

      List<string> expected = new List<string>(new string[] { "one", "two", "three", "four", "five" });

      appReg.WriteStringList(keyName, expected);

      Assert.That(appReg.ReadStringList(keyName), Is.EqualTo(expected));
    }
    catch (Exception ex)
    {
      Assert.Fail(ex.Message);
    }
  }

  [Test]
  public void ReadWriteIntList()
  {
    try
    {
      var appReg = createAppReg();

      string keyName = "IntegerListTest";
      Assert.That(appReg.ReadList<int>(keyName).Count, Is.EqualTo(0));

      List<int> expected = new List<int>(new int[] { -2, -1, 0, 1, 2 });

      appReg.WriteList(keyName, expected);

      Assert.That(appReg.ReadList<int>(keyName), Is.EqualTo(expected));
    }
    catch (Exception ex)
    {
      Assert.Fail(ex.Message);
    }
  }

  [Test]
  public void ReadWriteToolStripItems()
  {
    try
    {
      var appReg = createAppReg();

      string keyName = "ToolStripItemsTest";
      ToolStrip ts = new ToolStrip();
      ToolStripItemCollection tsic = new ToolStripItemCollection(ts, new ToolStripItem[0]);
      appReg.ReadToolStripItems(keyName, tsic);

      Assert.That(tsic == null || tsic.Count == 0, Is.True);

      tsic = new ToolStripItemCollection(ts, new ToolStripItem[] { new ToolStripMenuItem("One"), new ToolStripMenuItem("Two"), new ToolStripMenuItem("Three") });

      appReg.WriteToolStripItems(keyName, tsic);

      ToolStripItemCollection newTSIC = new ToolStripItemCollection(ts, new ToolStripItem[0]);

      appReg.ReadToolStripItems(keyName, newTSIC);

      Assert.That(newTSIC.Count, Is.EqualTo(tsic.Count));

      for (int index = 0; index < tsic.Count; index++)
      {
        Assert.That(newTSIC[index].Text, Is.EqualTo(tsic[index].Text));
      }
    }
    catch (Exception ex)
    {
      Assert.Fail(ex.Message);
    }
  }

  [Test]
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

      Assert.That(testForm2.Width, Is.EqualTo(testForm1.Width));
      Assert.That(testForm2.Height, Is.EqualTo(testForm1.Height));
      Assert.That(testForm2.Top, Is.EqualTo(testForm1.Top));
      Assert.That(testForm2.Left, Is.EqualTo(testForm1.Left));
      Assert.That(testForm2.WindowState, Is.EqualTo(testForm1.WindowState));
    }
    catch (Exception ex)
    {
      Assert.Fail(ex.Message);
    }
  }

  [Test]
  public void ReadWriteBytes()
  {
    try
    {
      var appReg = createAppReg();

      string keyName = "BinaryTest";

      Assert.That(appReg.ReadBytes(keyName), Is.Null);

      byte[] bytes = Encoding.ASCII.GetBytes(keyName);
      appReg.WriteBytes(keyName, bytes);

      byte[]? readBytes = appReg.ReadBytes(keyName);

      Assert.That(readBytes, Is.Not.Null);

      string readString = Encoding.UTF8.GetString(readBytes!);

      Assert.That(readString, Is.EqualTo(keyName));
    }
    catch (Exception ex)
    {
      Assert.Fail(ex.Message);
    }
  }

  [Test]
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

      Assert.That(newTO, Is.EqualTo(to));

      appReg.WriteString(keyName, "Invalid Object");
      newTO = appReg.ReadObject<TestObject>(keyName);
      Assert.That(newTO, Is.Null);
    }
    catch (Exception ex)
    {
      Assert.Fail(ex.Message);
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