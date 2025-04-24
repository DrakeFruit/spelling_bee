namespace Bee;

[GameResource("Word List", "wl", "A list of words and definitions", Category = "Bee", Icon = "list")]
public class WordList : GameResource
{
	public string Word { get; set; }
	public string Definition { get; set; }
}
