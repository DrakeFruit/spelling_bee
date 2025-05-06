using System;
using System.Threading.Tasks;
using Sandbox.Audio;
using Sandbox.Network;
using TikTokTTS;

namespace Bee;

public partial class GameManager : Component, Component.INetworkListener
{
	[Property] public WordListFile ListFile { get; set; }
	[Sync] public static string CurrentWord { get; set; }
	[Sync] public static string CurrentDefinition { get; set; }
	
	protected override void OnFixedUpdate()
	{
		
	}
	
	// public void NewWord()
	// {
	// 	if ( Connection.Local != CurrentPlayer.Network.Owner ) return;
	// 	var element = List.Words.ElementAt( Game.Random.Int( 0, List.Words.Count - 1 ) );
	// 	CurrentWord = element.Key;
	// 	CurrentDefinition = element.Value;
	// 	
	// 	//var audioPlayer = TikTokTTS.TikTokTTS.Say( "spell " + CurrentWord, "en_male_ukneighbor" );
	// }
	
	public enum CurrentGameState
	{
		
	}
}
