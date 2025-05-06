using System;
using System.Threading.Tasks;
using Sandbox.Audio;
using Sandbox.Network;
using TikTokTTS;

namespace Bee;

public partial class GameManager
{
	[Sync] public static NetList<PlayerControllerExtras> Players { get; set; } = new();
	
	[Property] public bool StartServer { get; set; } = true;
	[Property] public GameObject PlayerPrefab { get; set; }
	[Property] public List<GameObject> SpawnPoints { get; set; }

	protected override async Task OnLoad()
	{
		if ( Scene.IsEditor )
			return;

		if ( StartServer && !Networking.IsActive )
		{
			LoadingScreen.Title = "Creating Lobby";
			await Task.DelayRealtimeSeconds( 0.1f );
			Networking.CreateLobby( new() );
		}
	}
	
	public void OnActive( Connection channel )
	{
		Log.Info( $"Player '{channel.DisplayName}' has joined the game" );

		if ( !PlayerPrefab.IsValid() )
			return;

		var startLocation = FindSpawnLocation().WithScale( 1 );

		var player = PlayerPrefab.Clone( startLocation, name: $"Player - {channel.DisplayName}" );
		player.NetworkSpawn( channel );
		Players.Add( player.GetComponent<PlayerControllerExtras>() );
	}

	public void OnDisconnected( Connection channel )
	{
		Players.Remove( Players.FirstOrDefault( x => x.Network.Owner == channel ) );
	}
	
	Transform FindSpawnLocation()
	{
		if ( SpawnPoints is not null && SpawnPoints.Count > 0 )
		{
			return Random.Shared.FromList( SpawnPoints ).WorldTransform;
		}
		
		var spawnPoints = Scene.GetAllComponents<SpawnPoint>().ToArray();
		if ( spawnPoints.Length > 0 )
		{
			return Random.Shared.FromArray( spawnPoints ).WorldTransform;
		}
		
		return WorldTransform;
	}
}
