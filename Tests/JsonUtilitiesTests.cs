using System.Diagnostics.CodeAnalysis;
using Tests.Resources;
using TNT.Utilities;

namespace Tests;

[ExcludeFromCodeCoverage]
internal class JsonUtilitiesTests
{
	//[Test]
	//public void JsonUtilities_serialize()
	//{
	//	var sut = new ExtExtBaseClass1();
	//	var result = JsonUtilities.serialize(sut);
	//	Assert.That(result, Is.EqualTo("{\"MyLong\":0,\"e1IntProperty\":0,\"e1StringProperty\":null,\"baseIntProperty\":0,\"baseStringProperty\":null}"));
	//}

	[Test]
	public void JsonUtilities_serialize_deserialize()
	{
		var sut = new ListClass
		{
			new ExtExtBaseClass1(),
			new ExtBaseClass1(),
			new BaseClass()
		};
		var json = JsonUtilities.serializeObject(sut);
		var deserializedSut = JsonUtilities.deserializeJson<ListClass>(json);
		Assert.That(deserializedSut, Is.EqualTo(sut));
	}

	//[Test]
	//public void JsonUtilities_deserialize()
	//{
	//	var sut = new ExtExtBaseClass1();
	//	var result = JsonUtilities.serialize(sut);
	//	var newSut = JsonUtilities.deserialize<ExtExtBaseClass1>(result);
	//	Assert.That(newSut, Is.EqualTo(sut));
	//}
}
