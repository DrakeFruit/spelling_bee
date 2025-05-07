namespace Bee;

[GameResource("Word List", "wl", "A list of words and definitions", Category = "Bee", Icon = "list")]
public class WordListFile : GameResource
{
	[Hide] public Dictionary<string, string> Words { get; set; }
}
