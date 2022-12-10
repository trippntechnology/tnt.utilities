using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;

namespace TNT.Utilities
{
	/// <summary>
	/// Class to interact with the Window's registry
	/// </summary>
	public class ApplicationRegistry
	{
		/// <summary>
		/// Base registry key
		/// </summary>
		public RegistryKey BaseRegistryKey { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseRegistryKey">Base registry key</param>
		/// <param name="companyName">Company name to use in the sub-key</param>
		/// <param name="appName">Application name to use in the sub-key</param>
		public ApplicationRegistry(RegistryKey baseRegistryKey, string companyName, string appName)
		{
			BaseRegistryKey = baseRegistryKey.CreateSubKey("SOFTWARE");

			// Make sure the company name registry key is there
			BaseRegistryKey = BaseRegistryKey.CreateSubKey(companyName);

			// Make sure the appName key exists
			BaseRegistryKey = BaseRegistryKey.CreateSubKey(appName);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="form"><see cref="Form"/> who's state should be stored</param>
		/// <param name="baseRegistryKey">Base registry key</param>
		/// <param name="companyName">Company name to use in the sub-key</param>
		/// <param name="appName">Application name to use in the sub-key</param>
		public ApplicationRegistry(Form form, RegistryKey baseRegistryKey, string companyName, string appName)
			: this(baseRegistryKey, companyName, appName)
		{
			form.FormClosing += FormClosing;
			form.Load += FormLoad;
		}

		[ExcludeFromCodeCoverage]
		private void FormLoad(object? sender, EventArgs e) => (sender as Form)?.Also(it => LoadState(it));

		[ExcludeFromCodeCoverage]
		private void FormClosing(object? sender, FormClosingEventArgs e) => (sender as Form)?.Also(it => SaveState(it));

		#region Save/Load Form State

		private void SaveState(Form form)
		{
			if (form != null)
			{
				string subKey = string.Format("FormState\\{0}", form.Name);

				WriteInteger(subKey, "WindowState", (int)form.WindowState);

				if (form.WindowState == FormWindowState.Normal)
				{
					WriteInteger(subKey, "Width", form.Width);
					WriteInteger(subKey, "Height", form.Height);
					WriteInteger(subKey, "Top", form.Top);
					WriteInteger(subKey, "Left", form.Left);
				}
			}
		}

		private void LoadState(Form form)
		{
			if (form != null)
			{
				string subKey = string.Format("FormState\\{0}", form.Name);

				form.Width = ReadInteger(subKey, "Width", form.Width);
				form.Height = ReadInteger(subKey, "Height", form.Height);
				form.Top = ReadInteger(subKey, "Top", form.Top);
				form.Left = ReadInteger(subKey, "Left", form.Left);

				form.WindowState = (FormWindowState)ReadInteger(subKey, "WindowState", (int)form.WindowState);
			}
		}

		#endregion

		#region Read/Write Integer

		/// <summary>
		/// Writes an integer value associated with name
		/// </summary>
		/// <param name="name">Name associated with the value</param>
		/// <param name="value">Integer value</param>
		public void WriteInteger(string name, int value)
		{
			BaseRegistryKey.SetValue(name, value);
		}

		/// <summary>
		/// Writes an integer value associated with name at a subkey specified by the SubKey
		/// </summary>
		/// <param name="subKey">Subkey where to write the value</param>
		/// <param name="name">Name associated with the value</param>
		/// <param name="value">Integer value</param>
		public void WriteInteger(string subKey, string name, int value)
		{
			RegistryKey rk = BaseRegistryKey.CreateSubKey(subKey);
			rk.SetValue(name, value);
		}

		/// <summary>
		/// Reads and integer value given name 
		/// </summary>
		/// <param name="name">Name of associated with value</param>
		/// <param name="defValue">Default value to be used if registry entry doesn't exist</param>
		/// <returns>Value associated with the name</returns>
		public int ReadInteger(string name, int defValue)
		{
			object? obj = BaseRegistryKey.GetValue(name);

			if (obj != null)
			{
				defValue = Convert.ToInt32(obj);
			}

			return defValue;
		}

		/// <summary>
		/// Reads and integer value given the subkey and name 
		/// </summary>
		/// <param name="subKey">Subkey where value is located</param>
		/// <param name="name">Name of associated with value</param>
		/// <param name="defValue">Default value to be used if registry entry doesn't exist</param>
		/// <returns>Value associated with the subkey and name</returns>
		public int ReadInteger(string subKey, string name, int defValue)
		{
			RegistryKey? rk = BaseRegistryKey.OpenSubKey(subKey);

			if (rk != null)
			{
				object? obj = rk.GetValue(name);

				if (obj != null)
				{
					defValue = Convert.ToInt32(obj);
				}
			}

			return defValue;
		}

		#endregion

		#region Read/Write String

		/// <summary>
		/// Writes a string value to the name location
		/// </summary>
		/// <param name="name">Name associated with the value</param>
		/// <param name="value">String value</param>
		public void WriteString(string name, string value)
		{
			BaseRegistryKey.SetValue(name, value);
		}

		/// <summary>
		/// Writes a string value to the subkey and name location
		/// </summary>
		/// <param name="subKey">Subkey where name is located</param>
		/// <param name="name">Name associated with the value</param>
		/// <param name="value">String value</param>
		public void WriteString(string subKey, string name, string value)
		{
			RegistryKey rk = BaseRegistryKey.CreateSubKey(subKey);
			rk.SetValue(name, value);
		}

		/// <summary>
		/// Reads a string value at the name location
		/// </summary>
		/// <param name="name">Name associated with the value</param>
		/// <param name="defValue">Default value to return if entry is missing</param>
		/// <returns>Value associated with the name</returns>
		public string ReadString(string name, string defValue)
		{
			object? obj = BaseRegistryKey.GetValue(name);
			defValue = obj?.ToString() ?? defValue;

			return defValue;
		}

		/// <summary>
		/// Reads a string value at the subkey and name location
		/// </summary>
		/// <param name="subKey">Subkey where value is located</param>
		/// <param name="name">Name associated with the value</param>
		/// <param name="defValue">Default value to return if entry is missing</param>
		/// <returns>Value associated with the subkey and name</returns>
		public string ReadString(string subKey, string name, string defValue)
		{
			RegistryKey? rk = BaseRegistryKey.OpenSubKey(subKey);

			if (rk != null)
			{
				object? obj = rk.GetValue(name);
				defValue = obj?.ToString() ?? defValue;
			}

			return defValue;
		}

		#endregion

		#region Read/Write Boolean

		/// <summary>
		/// Writes a boolean value given name
		/// </summary>
		/// <param name="name">Name associated with the value</param>
		/// <param name="value">Boolean value to write</param>
		public void WriteBoolean(string name, bool value)
		{
			BaseRegistryKey.SetValue(name, value);
		}

		/// <summary>
		/// Writes a boolean value given a subkey and name
		/// </summary>
		/// <param name="subKey">Subkey where the name exists</param>
		/// <param name="name">Name associated with the value</param>
		/// <param name="value">Boolean value to write</param>
		public void WriteBoolean(string subKey, string name, bool value)
		{
			RegistryKey rk = BaseRegistryKey.CreateSubKey(subKey);
			rk.SetValue(name, value);
		}

		/// <summary>
		/// Get a boolean value associates with a name
		/// </summary>
		/// <param name="name">Name associated with the value</param>
		/// <param name="defValue">Default value to use if entry doesn't exist in registry</param>
		/// <returns>Boolean value associated with name</returns>
		public bool ReadBoolean(string name, bool defValue)
		{
			object? obj = BaseRegistryKey.GetValue(name);

			if (obj != null)
			{
				defValue = Convert.ToBoolean(obj);
			}

			return defValue;
		}

		/// <summary>
		/// Get a boolean value associates with a subkey and name
		/// </summary>
		/// <param name="subKey">Subkey where the name is located</param>
		/// <param name="name">Name associated with the value</param>
		/// <param name="defValue">Default value to use if entry doesn't exist in registry</param>
		/// <returns>Boolean value associated with subkey and name</returns>
		public bool ReadBoolean(string subKey, string name, bool defValue)
		{
			RegistryKey? rk = BaseRegistryKey.OpenSubKey(subKey);

			if (rk != null)
			{
				object? obj = rk.GetValue(name);

				if (obj != null)
				{
					defValue = Convert.ToBoolean(obj);
				}
			}

			return defValue;
		}

		#endregion

		#region Read/Write StringList

		/// <summary>
		/// Writes a list of strings at the key specified by name
		/// </summary>
		/// <param name="name">Key where list is written</param>
		/// <param name="list">List of strings</param>
		public void WriteStringList(string name, List<string> list)
		{
			WriteList<string>(name, list);
		}

		/// <summary>
		/// Reads a list of string found at the key with name
		/// </summary>
		/// <param name="name">Name of the key</param>
		/// <returns>List of string found within the key</returns>
		public List<string> ReadStringList(string name)
		{
			return ReadList<string>(name);
		}

		#endregion

		#region Read/Write List<>

		/// <summary>
		/// Writes a list at the key specified by name
		/// </summary>
		/// <param name="name">Key where list is written</param>
		/// <param name="list">List of type T</param>
		/// <typeparam name="T">Type of objects within list</typeparam>
		public void WriteList<T>(string name, List<T> list)
		{
			try
			{
				// Clear the current list
				BaseRegistryKey.DeleteSubKeyTree(name);
			}
			catch { }

			RegistryKey rk = BaseRegistryKey.CreateSubKey(name);

			// Added the list
			for (int index = 0; index < list.Count; index++)
			{
				rk.SetValue(index.ToString(), list[index]!);
			}
		}

		/// <summary>
		/// Reads a list found at the key with name
		/// </summary>
		/// <param name="name">Name of the key</param>
		/// <typeparam name="T">Type of objects within list</typeparam>
		/// <returns>List of type T found within the key</returns>
		public List<T> ReadList<T>(string name)
		{
			List<T> list = new List<T>();

			RegistryKey? rk = BaseRegistryKey.OpenSubKey(name);

			if (rk != null)
			{
				int valueIndex = 0;
				object? value = rk.GetValue(valueIndex.ToString());

				while (value != null)
				{
					list.Add((T)value);
					valueIndex++;
					value = rk.GetValue(valueIndex.ToString());
				}
			}

			return list;
		}

		#endregion

		#region Read/Write ToolStripItemCollection

		/// <summary>
		/// Writes the Text fields of the item to the registry key name
		/// </summary>
		/// <param name="name">Name of registry key</param>
		/// <param name="items">Item whose Text fields should be written to the key</param>
		public void WriteToolStripItems(string name, ToolStripItemCollection items)
		{
			List<string> list = new List<string>();

			foreach (ToolStripItem tsi in items)
			{
				list.Add(tsi.Text);
			}

			WriteStringList(name, list);
		}

		/// <summary>
		/// Reads the items saved by WriteToolStripItems and create a new ToolStripMenuItem using
		/// the value
		/// </summary>
		/// <param name="name">Name of key where items are written</param>
		/// <param name="tsic">Collection that can be populated with the items</param>
		public void ReadToolStripItems(string name, ToolStripItemCollection tsic)
		{
			List<string> list = ReadStringList(name);

			tsic.Clear();

			foreach (string str in list)
			{
				tsic.Add(new ToolStripMenuItem(str));
			}
		}

		#endregion

		#region Read/Write Bytes

		/// <summary>
		/// Writes a byte array to the registery
		/// </summary>
		/// <param name="name">Name of registry key</param>
		/// <param name="bytes">Byte array</param>
		public void WriteBytes(string name, byte[] bytes)
		{
			BaseRegistryKey.SetValue(name, bytes);
		}

		/// <summary>
		/// Reads a byte array from the registry
		/// </summary>
		/// <param name="name">Name of registry key</param>
		/// <returns>Byte array</returns>
		public byte[]? ReadBytes(string name)
		{
			return BaseRegistryKey.GetValue(name) as Byte[];
		}

		#endregion

		#region Read/Write Object

		/// <summary>
		/// Writes a object to the register
		/// </summary>
		/// <param name="name">Name of registry key</param>
		/// <param name="obj">Object to write</param>
		[Obsolete("BinaryFormatter.Deserialize is Obsolete. It is recommended other serialization methods be used.")]
		public void WriteObject(string name, object obj)
		{
			byte[] bytes = Utilities.ToByteArray(obj);
			WriteBytes(name, bytes);
		}

		/// <summary>
		/// Reads an object from the registry
		/// </summary>
		/// <typeparam name="T">Type of object</typeparam>
		/// <param name="name">Name of registry key</param>
		/// <returns>Object from the registry</returns>
		[Obsolete("BinaryFormatter.Deserialize is Obsolete. It is recommended other serialization methods be used.")]
		public T? ReadObject<T>(string name)
		{
			byte[]? bytes = ReadBytes(name);
			return Utilities.FromByteArray<T>(bytes);
		}

		#endregion
	}
}