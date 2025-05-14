using System;
using System.Threading.Tasks;
using Sandbox.Audio;
using Sandbox.Network;
using Sandbox.Rendering;
using Sandbox.Services;
using TikTokTTS;

namespace Bee;

public partial class GameManager : Component, Component.INetworkListener
{
	[Property] public WordListFile ListFile { get; set; }
	[Property] public TextRenderer GuessDisplay { get; set; }
	
	[Sync] public string CurrentWord { get; set; }
	[Sync] public string CurrentDefinition { get; set; }
	[Sync] public string CurrentGuess { get; set; }
	[Sync] public PlayerControllerExtras CurrentPlayer { get; set; }
	[Sync] public GameState currentGameState { get; set; } = GameState.Waiting;
	[Sync] public TimeSince TimeSinceRoundStarted { get; set; }
	
	GuessInput guessInput { get; set; }

	protected override void OnStart()
	{
		CurrentWord = null;
		CurrentDefinition = null;
	}
	
	protected override void OnFixedUpdate()
	{
		switch ( currentGameState )
		{
			case GameState.Waiting:
				Waiting();
				break;
			case GameState.Playing:
				Playing();
				break;
			case GameState.SwitchingPlayers:
				SwitchingPlayers();
				break;
		}
	}

	void Waiting()
	{
		if ( Players.Count >= 2 ) currentGameState = GameState.SwitchingPlayers;
	}

	void Playing()
	{
		if ( Connection.Local == Connection.Host && guessInput.IsValid() && guessInput.Network.Owner == CurrentPlayer.Network.Owner )
		{
			CurrentGuess = guessInput.Guess;
		}
		
		GuessDisplay.Text = CurrentGuess;
		if ( !CurrentPlayer.IsValid() ) currentGameState = GameState.SwitchingPlayers;
	}

	void SwitchingPlayers()
	{
		if ( Connection.Local == Connection.Host )
		{
			if ( guessInput != null )
			{
				guessInput.SetMyTurn(false);
				CurrentPlayer.SetCanMove( true );
				CurrentPlayer.MoveTo( Vector3.Zero, Rotation.Identity );
			}

			CurrentPlayer = CurrentPlayer == null ? Players.FirstOrDefault( x => x.Alive ) :
				Players.FirstOrDefault( x => x.Alive && Players.IndexOf( x ) > Players.IndexOf( CurrentPlayer ) );
			
			if ( CurrentPlayer == null ) CurrentPlayer = Players.FirstOrDefault( x => x.Alive );
			Log.Info("Switching to: " + CurrentPlayer.Network.Owner.DisplayName);
			
			CurrentPlayer.SetCanMove( false );
			CurrentPlayer.MoveTo( Vector3.Zero + Vector3.Right * 150, Rotation.Identity );
		}
		
		guessInput = CurrentPlayer?.GameObject.Components.Get<GuessInput>();
		guessInput?.SetMyTurn( true );
		
		NewWord();
		currentGameState = GameState.Playing;
	}

	public async Task Submit()
	{
		//Sound.Play();
		if ( guessInput?.Guess.ToLower() == CurrentWord?.ToLower() )
		{
			if ( CurrentPlayer.Network.Owner == Connection.Local ) SetDisplayColor( "green" );
			GuessDisplay.Color = Color.Green;
		}
		else
		{
			if ( CurrentPlayer.Network.Owner == Connection.Local ) SetDisplayColor( "red" );
			GuessDisplay.Color = Color.Red;
		}

		//Sound.Play();
		await Task.DelayRealtimeSeconds( 1 );
		guessInput.Entry.Text = "";
		SetDisplayColor( "white" );
		
		currentGameState = GameState.SwitchingPlayers;
	}

	[Rpc.Broadcast]
	public void SetDisplayColor( string color )
	{
		guessInput?.Entry?.SetProperty( "style", "color: " + color );
		GuessDisplay.Color = Color.White;
	}
	
	public void NewWord()
	{
		if ( Connection.Local != Connection.Host ) return;
		var element = ListFile.Words.ElementAt( Game.Random.Int( 0, ListFile.Words.Count - 1 ) );
		CurrentWord = element.Key;
		CurrentDefinition = element.Value;

		//var audioPlayer = TikTokTTS.TikTokTTS.Say( "spell " + CurrentWord, "en_male_ukneighbor" );
	}

	public enum GameState
	{
		Waiting,
		Playing,
		SwitchingPlayers
	}
}
