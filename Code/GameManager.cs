using System;
using System.Threading.Tasks;
using Sandbox.Audio;
using TikTokTTS;

namespace Bee;

public sealed class GameManager : Component
{
	[Property] public WordListFile ListFile { get; set; }
	public static string CurrentWord { get; set; }
	public static string CurrentDefinition { get; set; }
	public static WordListFile List { get; set; }
	
	protected override void OnStart()
	{
		List = ListFile;
		NewWord();
	}
	
	protected override void OnFixedUpdate()
	{
		
	}

	public static void NewWord()
	{
		var element = List.Words.ElementAt( Game.Random.Int( 0, List.Words.Count - 1 ) );
		CurrentWord = element.Key;
		CurrentDefinition = element.Value;
		
		var audioPlayer = TikTokTTS.TikTokTTS.Say( "spell " + CurrentWord, "en_male_ukneighbor" );
	}
}
