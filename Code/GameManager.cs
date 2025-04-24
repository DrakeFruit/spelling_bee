using System;
using System.IO;

public sealed class GameManager : Component
{
	[Property] public List<Word> WordList { get; set; }
	public static Word CurrentWord { get; set; }
	
	protected override void OnStart()
	{
		CurrentWord = WordList[ Game.Random.Int( 0, 1 ) ];
	}
	
	protected override void OnFixedUpdate()
	{
		
	}

	public struct Word
	{
		public string word { get; set; }
		public string definition { get; set; }
	}
}
