using System;
using System.Threading.Tasks;
using Sandbox.Audio;
using Sandbox.Network;
using Sandbox.Rendering;
using TikTokTTS;

namespace Bee;

public partial class GameManager : Component, Component.INetworkListener
{
	[Property] public WordListFile ListFile { get; set; }
	[Property] public TextRenderer GuessDisplay { get; set; }
	[Sync] public string CurrentWord { get; set; }
	[Sync] public string CurrentDefinition { get; set; }
	
	[Sync] public PlayerControllerExtras CurrentPlayer { get; set; }
	[Sync] public string CurrentGuess { get; set; }
	[Sync] public bool GameStarted { get; set; }
	GuessInput guessInput { get; set; }

	protected override void OnStart()
	{
		CurrentWord = null;
		CurrentDefinition = null;
	}
	
	protected override void OnFixedUpdate()
	{
		if ( Players.Count >= 2 ) GameStarted = true;
		if ( !GameStarted ) return;
		
		if ( Connection.Local == Connection.Host )
		{
			if ( CurrentPlayer == null )
			{
				CurrentPlayer = Players[0];
				CurrentPlayer.WorldPosition = Vector3.Zero;
				CurrentPlayer.CanMove = false;
				guessInput ??= CurrentPlayer.GameObject.Components.Get<GuessInput>();
				NewWord();
			}
			else
			{
				guessInput.MyTurn = true;
				CurrentGuess = guessInput.Entry.Text;
			}
		}
		
		GuessDisplay.Text = CurrentGuess;
	}
	
	public void NewWord()
	{
		if ( Connection.Local != CurrentPlayer.Network.Owner ) return;
		var element = ListFile.Words.ElementAt( Game.Random.Int( 0, ListFile.Words.Count - 1 ) );
		CurrentWord = element.Key;
		CurrentDefinition = element.Value;

		//var audioPlayer = TikTokTTS.TikTokTTS.Say( "spell " + CurrentWord, "en_male_ukneighbor" );
	}
	
	public enum CurrentGameState
	{
		
	}
}
