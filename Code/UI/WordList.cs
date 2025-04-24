namespace Bee;

[GameResource("Word List", "wl", "A list of words and definitions", Category = "Bee", Icon = "list")]
public class WordListFile : GameResource
{
	public List<WordPair> Words { get; set; }
	
	public struct WordPair
	{
		public string Word { get; set; }
		public string Definition { get; set; }
	}
}
