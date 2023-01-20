using Newtonsoft.Json;
using System.Text.Json;

namespace TNT.Utilities;

/// <summary>
/// Json utility methods
/// </summary>
public static class JsonUtilities
{
	///// <summary>
	///// Wrapper around <see cref="System.Text.Json.JsonSerializer.Serialize(object?, Type, JsonSerializerOptions?)"/>
	///// </summary>
	///// <returns><see cref="string"/> representing <paramref name="obj"/></returns>
	//public static string serialize(object obj, JsonSerializerOptions? options = null) => System.Text.Json.JsonSerializer.Serialize(obj, options);

	///// <summary>
	///// Wrapper around <see cref="System.Text.Json.JsonSerializer.Deserialize{TValue}(string, JsonSerializerOptions?)"/>
	///// </summary>
	///// <typeparam name="T"><see cref="Type"/> returned</typeparam>
	///// <returns>Object of <see cref="Type"/> <typeparamref name="T"/></returns>
	//public static T? deserialize<T>(string json) => System.Text.Json.JsonSerializer.Deserialize<T>(json);

	/// <summary>
	/// Wrapper around <see cref="JsonConvert.SerializeObject(object?, JsonSerializerSettings)"/>
	/// </summary>
	/// <returns><see cref="string"/> representing <paramref name="obj"/></returns>
	public static string serializeObject(object obj, JsonSerializerSettings? settings = null)
	{
		settings = settings ?? new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
		return JsonConvert.SerializeObject(obj, settings);
	}

	/// <summary>
	/// Wrapper around <see cref="JsonConvert.DeserializeObject(string, JsonSerializerSettings)"/>
	/// </summary>
	/// <typeparam name="T"><see cref="Type"/> returned</typeparam>
	/// <returns>Object of <see cref="Type"/> <typeparamref name="T"/></returns>
	public static T? deserializeJson<T>(string json, JsonSerializerSettings? settings = null)
	{
		settings = settings ?? new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
		return JsonConvert.DeserializeObject<T>(json, settings);
	}
}