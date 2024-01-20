using System.Diagnostics.CodeAnalysis;
using Tests.Resources;
using TNT.Utilities;

namespace Tests;

[ExcludeFromCodeCoverage]
internal class JsonUtilitiesTests
{
  [Test]
  public void JsonUtilities_serialize_deserialize()
  {
    var sut = new JsonTestClass()
    {
      ListExample = new List<BaseClass> {
        new ExtExtBaseClass1(){ baseIntProperty =3 , baseStringProperty = "three", e1IntProperty = 33, e1StringProperty = "thirty-three", MyLong = 333L},
        new ExtBaseClass1() { baseIntProperty=2, baseStringProperty = "Two", e1IntProperty = 22, e1StringProperty = "twenty-two"},
        new BaseClass(){baseIntProperty = 1, baseStringProperty = "one"},
      },
      IntExample = 27,
      StringExample = "twenty-seven"
    };
    string json = JsonUtilities.serializeObject(sut);
    var deserializedSut = JsonUtilities.deserializeJson<JsonTestClass>(json);
    Assert.That(deserializedSut, Is.EqualTo(sut));
  }
}
