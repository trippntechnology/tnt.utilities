﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace TNT.Utilities
{
	/// <summary>
	/// Contains miscellaneous method that might be used across applications
	/// </summary>
	public static class Utilities
	{
		/// <summary>
		/// Serializes an object into an XML string 
		/// </summary>
		/// <typeparam name="T">Type of object being serialized</typeparam>
		/// <param name="obj">Object to serialize</param>
		/// <param name="expectedTypes">List of types that can be provided. This list typically included types of 
		/// classes that derived from base classes referenced in the object.</param>
		/// <returns>XML representation of the object</returns>
		public static string Serialize<T>(T obj, Type[] expectedTypes)
		{
			using (StringWriter sw = new StringWriter())
			using (XmlTextWriter tw = new XmlTextWriter(sw))
			{
				tw.Formatting = Formatting.Indented;

				XmlSerializer ser = new XmlSerializer(typeof(T), expectedTypes);
				ser.Serialize(tw, obj);

				return sw.ToString();
			}
		}

		/// <summary>
		/// De-serializes an XML representation of an object
		/// </summary>
		/// <typeparam name="T">Type of object represented by the XML</typeparam>
		/// <param name="content">XML content</param>
		/// <param name="expectedTypes">Expected types referenced during the deserialization</param>
		/// <returns>Object created from the content</returns>
		public static T Deserialize<T>(string content, Type[] expectedTypes)
		{
			using (StringReader sr = new StringReader(content))
			using (XmlTextReader tr = new XmlTextReader(sr))
			{
				XmlSerializer deser = new XmlSerializer(typeof(T), expectedTypes);
				return (T)deser.Deserialize(tr);
			}
		}

		/// <summary>
		/// Serializes <paramref name="obj"/> to the file specified by <paramref name="fileName"/>
		/// </summary>
		/// <typeparam name="T">Type of <paramref name="obj"/></typeparam>
		/// <param name="obj">Object to serialize</param>
		/// <param name="fileName">Name of file</param>
		public static void SerializeToFile<T>(T obj, string fileName)
		{
			using (TextWriter textWriter = new StreamWriter(fileName))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(textWriter, obj);
			}
		}

		/// <summary>
		/// De-serializes an object from disk
		/// </summary>
		/// <typeparam name="T">Type of object expected to be deserialized</typeparam>
		/// <param name="fileName">File where object resides</param>
		/// <returns>Object of type T </returns>
		public static T DeserializeFromFile<T>(string fileName)
		{
			using (TextReader textReader = new StreamReader(fileName))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				return (T)serializer.Deserialize(textReader);
			}
		}

		/// <summary>
		/// Converts an object to a byte array
		/// </summary>
		/// <param name="obj">Object to convert</param>
		/// <returns>Byte array representing the object</returns>
		public static byte[] ToByteArray(object obj)
		{
			byte[] bytes = null;

			using (MemoryStream ms = new MemoryStream())
			{
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(ms, obj);
				ms.Position = 0;
				bytes = new byte[ms.Length];
				ms.Read(bytes, 0, (int)ms.Length);
			}

			return bytes;
		}

		/// <summary>
		/// Returns an object of type T from the byte array
		/// </summary>
		/// <typeparam name="T">Type of object bytes represent</typeparam>
		/// <param name="bytes">Byte array of object</param>
		/// <returns>Object of type T from the byte array</returns>
		public static T FromByteArray<T>(byte[] bytes)
		{
			using (MemoryStream ms = new MemoryStream(bytes))
			{
				ms.Position = 0;
				IFormatter formatter = new BinaryFormatter();
				T obj = (T)formatter.Deserialize(ms);
				return obj;
			}
		}

		#region GetNameSpaceClasses

		/// <summary>
		/// Returns a list of class names contained within a name space
		/// </summary>
		/// <param name="nameSpace">Name space were classes reside</param>
		/// <param name="assemblyName">Assembly where name space exists</param>
		/// <returns>List of class names contained within a name space</returns>
		public static List<string> GetNameSpaceClasses(string nameSpace, string assemblyName)
		{
			return GetNameSpaceClasses(nameSpace, assemblyName, null);
		}

		/// <summary>
		/// Returns a list of class names contained within a name space with the specified base type
		/// </summary>
		/// <param name="nameSpace">Name space were classes reside</param>
		/// <param name="assemblyName">Assembly where name space exists</param>
		/// <param name="baseType">Base class of the class names returned</param>
		/// <returns>List of class names contained within a name space with the specified base type</returns>
		public static List<string> GetNameSpaceClasses(string nameSpace, string assemblyName, Type baseType)
		{
			List<string> classList = new List<string>();
			Assembly ass = Assembly.LoadFrom(assemblyName);

			classList.Add(string.Empty);

			if (ass != null)
			{
				classList.AddRange((from t in ass.GetTypes() where t.Namespace == nameSpace && !t.IsAbstract && t.InheritsFrom(baseType) select t.UnderlyingSystemType.FullName).ToList<string>());
			}

			return classList;
		}

		#endregion

		#region GetNameSpaceTypes

		/// <summary>
		/// Returns a list of types contained within a name space
		/// </summary>
		/// <param name="nameSpace">Name space where types reside</param>
		/// <param name="assemblyName">Assembly where name space exists</param>
		/// <returns>Array of types within the name space</returns>
		[Obsolete("Use GetTypes instead.")]
		public static Type[] GetNameSpaceTypes(string nameSpace, string assemblyName)
		{
			return GetNameSpaceTypes(nameSpace, assemblyName, null);
		}

		/// <summary>
		/// Returns a list of types contained within a name space with the specified base type
		/// </summary>
		/// <param name="nameSpace">Name space where types reside</param>
		/// <param name="assemblyName">Assembly where name space exists</param>
		/// <param name="baseType">Base type of the types returned</param>
		/// <returns>Array of types derived from the base type within the name space</returns>
		[Obsolete("Use GetTypes instead.")]
		public static Type[] GetNameSpaceTypes(string nameSpace, string assemblyName, Type baseType)
		{
			List<Type> types = new List<Type>();
			Assembly ass = Assembly.LoadFile(assemblyName);

			if (ass != null)
			{
				types = (from t in ass.GetTypes() where t.Namespace == nameSpace && !t.IsAbstract && t.InheritsFrom(baseType) select t).ToList<Type>();
			}

			return types.ToArray();
		}

		/// <summary>
		/// Returns a list of types matching <paramref name="filter"/> within assembly referenced by <paramref name="assemblyName"/>
		/// </summary>
		/// <param name="assemblyName">Name of assembly</param>
		/// <param name="filter"><see cref="Func{T, TResult}"/> used to filter the types returned</param>
		/// <returns>Array of types filtered by <paramref name="filter"/></returns>
		public static Type[] GetTypes(string assemblyName, Func<Type, bool> filter)
		{
			List<Type> types = new List<Type>();
			Assembly ass = Assembly.LoadFile(assemblyName);

			if (ass != null)
			{
				types = (from t in ass.GetTypes() where (filter != null && filter(t)) || filter == null select t).ToList();
			}

			return types.ToArray();
		}

		#endregion

		/// <summary>
		/// Gets the assembly attribute of the specified type
		/// </summary>
		/// <typeparam name="T">Attribute's type</typeparam>
		/// <param name="assembly">Assembly</param>
		/// <returns>Assembly attribute of the given type if exists, null otherwise</returns>
		public static T GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
		{
			if (assembly == null)
			{
				return null;
			}

			object[] attributes = assembly.GetCustomAttributes(typeof(T), true);

			if (attributes == null || attributes.Length == 0)
			{
				return null;
			}

			return (T)attributes[0];
		}

		/// <summary>
		/// Update most recently used listing
		/// </summary>
		/// <param name="splitButton">Button to add listing</param>
		/// <param name="fileName">Location of listing</param>
		public static void UpdateMRUListing(ToolStripSplitButton splitButton, string fileName)
		{
			// Add latest file
			splitButton.DropDownItems.Insert(0, new ToolStripMenuItem(fileName));

			List<string> list = new List<string>();

			// Create a list of MRU files
			foreach (ToolStripMenuItem tsmi in splitButton.DropDownItems)
			{
				list.Add(tsmi.Text);
			}

			// Get the first 10 distinct in the list
			list = (from str in list where !string.IsNullOrEmpty(str) select str).Distinct().Take(10).ToList();

			// Repopulate the MRU menu items
			splitButton.DropDownItems.Clear();

			foreach (string str in list)
			{
				splitButton.DropDownItems.Add(str);
			}
		}

		/// <summary>
		/// This method was added so that colorized cursors could be used. Currently, referencing a
		/// colorized cursor resource lowers the color count.
		/// </summary>
		/// <param name="resourceName">Reference to the resource</param>
		/// <returns>Cursor</returns>
		public static Cursor LoadColorCursor(string resourceName)
		{
			// Get the name of a temporary file
			string path = Path.GetTempFileName();

			// Put the resource into a stream and save it to a file
			using (Stream s = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName))
			using (FileStream fs = File.Create(path, (int)s.Length))
			{
				// Fill the bytes[] array with the stream data         
				byte[] bytesInStream = new byte[s.Length];
				s.Read(bytesInStream, 0, (int)bytesInStream.Length);

				// Use FileStream object to write to the specified file         
				fs.Write(bytesInStream, 0, bytesInStream.Length);
			}

			// Load the cursor from the file and delete file
			Cursor hand = new Cursor(LoadCursorFromFile(path));
			File.Delete(path);

			return hand;
		}

		[DllImport("user32.dll")]
		static extern IntPtr LoadCursorFromFile(string lpFileName);

		#region Private methods

		/// <summary>
		/// Checks if thisBaseType inherits from baseType.
		/// </summary>
		/// <param name="thisBaseType">Type that is being check</param>
		/// <param name="baseType">Base type we're lookin for</param>
		/// <returns></returns>
		public static bool InheritsFrom(this Type thisBaseType, Type baseType)
		{
			bool rtnValue = false;

			if (baseType == null)
			{
				// Since baseType isn't provided ignore check and return true
				rtnValue = true;
			}
			else
			{
				if (thisBaseType == baseType || thisBaseType.BaseType == baseType)
				{
					rtnValue = true;
				}
				else
				{
					// Check if base type is further down the inheritance list
					if (thisBaseType.BaseType.BaseType != null)
					{
						rtnValue = thisBaseType.BaseType.InheritsFrom(baseType);
					}
				}
			}

			return rtnValue;
		}

		#endregion
	}
}
