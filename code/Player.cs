using Sandbox;
using System;

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


	public override void Simulate( Client cl )
	{
		base.Simulate( cl );
		TickPlayerUse();
	}

	public override void Touch( Entity other )
	{
		base.Touch( other );
		FileSystem.Data.WriteAllText( "./jumps.txt", "0" );
	}

	public override void BuildInput( InputBuilder input )
	{
		base.BuildInput( input );
		if ( input.Pressed( InputButton.Jump ) )
		{
			if ( GroundEntity == null ) return;
			int jumps = FileSystem.Data.ReadAllText( "./jumps.txt" ).ToInt();
			int newjumps = jumps + 1;
			FileSystem.Data.WriteAllText( "./jumps.txt", newjumps.ToString() );
		}

	}
}
