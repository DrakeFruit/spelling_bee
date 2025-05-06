using Sandbox;

public sealed class PlayerControllerExtras : Component
{
	[RequireComponent] PlayerController Player { get; set; }
	[Property] int WalkSpeed { get; set; } = 250;
	[Property] int JumpForce { get; set; } = 300;
	public bool Alive = true;
	protected override void OnStart()
	{
		Mouse.Visibility = MouseVisibility.Visible;
		Player.EyeAngles = Rotation.LookAt( Scene.Camera.WorldPosition - WorldPosition ).Angles().WithPitch( 0 );
	}
	
	protected override void OnFixedUpdate()
	{
		Player.WishVelocity = Input.AnalogMove * Scene.Camera.WorldRotation * WalkSpeed;
		if ( Player.Velocity.Length > 1 )
		{
			Player.EyeAngles = Player.EyeAngles.LerpTo( Rotation.LookAt( Player.Velocity ), 0.2f ) ;
		}
		if ( Input.Pressed( "Jump" ) && Player.IsOnGround ) Player.Jump( Vector3.Up * JumpForce );
	}
}
