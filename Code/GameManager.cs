using System;
using System.IO;

namespace Bee;

public sealed class GameManager : Component
{
	[Property] public WordListFile List { get; set; }
	public static WordListFile.WordPair CurrentWord { get; set; }
	
	protected override void OnStart()
	{
		CurrentWord = List.Words[ Game.Random.Int( 0, List.Words.Count - 1 ) ];
		Log.Info(CurrentWord);
	}
	
	protected override void OnFixedUpdate()
	{
		
	}
}
