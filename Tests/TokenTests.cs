using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using TNT.Utilities;

namespace Tests;

[ExcludeFromCodeCoverage]
public class TokenTests
{
	[Test]
	public void Token_Create_Size()
	{
		string token = Token.Create(8);
		Assert.That(token.Length, Is.EqualTo(8));

		token = Token.Create(15);
		Assert.That(token.Length, Is.EqualTo(15));
	}

	[Test]
	public void Token_Create_Sections()
	{
		string token = Token.Create(1, 4);
		Assert.That(Regex.IsMatch(token, "^[a-zA-Z0-9]{4}$"), Is.True);

		token = Token.Create(2, 4);
		Assert.That(Regex.IsMatch(token, "^[a-zA-Z0-9]{4}(-[a-zA-Z0-9]{4}){1}$"), Is.True);

		token = Token.Create(3, 4);
		Assert.That(Regex.IsMatch(token, "^[a-zA-Z0-9]{4}(-[a-zA-Z0-9]{4}){2}$"), Is.True);
	}

	[Test]
	public void Token_Create_Length_0()
	{
		string token = Token.Create(0);
		Assert.That(string.IsNullOrEmpty(token), Is.True);
	}

	[Test]
	public void Token_Create_Section_Count_0()
	{
		string token = Token.Create(0, 4);
		Assert.That(string.IsNullOrEmpty(token), Is.True);
	}

	[Test]
	public void Token_Create_Not_Equals_Sections()
	{
		string token = Token.Create(2, 4);
		string[] sections = token.Split('-');

		Assert.That(sections[1], Is.Not.EqualTo(sections[0]));
	}

	[Test]
	public void Token_Using_Custom_Chars()
	{
		char[] customCharacters = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

		string token = Token.Create(5, customCharacters);
		int output;

		Assert.That(int.TryParse(token, out output), Is.True);
	}
}
