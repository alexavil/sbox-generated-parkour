using Sandbox;

partial class Player : Sandbox.Player
{
	public override void Respawn()
	{

		SetModel( "models/citizen/citizen.vmdl" );

		// Use WalkController for movement (you can make your own PlayerController for 100% control)
		Controller = new WalkController();

		// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
		Animator = new StandardPlayerAnimator();

		// Use ThirdPersonCamera (you can make your own Camera for 100% control)
		Camera = new FirstPersonCamera();

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;



		base.Respawn();
	}
	public override void StartTouch( Entity other )
	{
		if ( GroundEntity != null )
		{
			ConsoleSystem.Run( "spawn_platform" );
		}
	}

}
