
using MinimalExample;
using Sandbox;
using System;

namespace ParkourGame
{

	public partial class MinimalGame : Sandbox.Game
	{
		public MinimalGame()
		{

			if ( IsServer )
			{
				new MinimalHudEntity();
			}

			if ( IsClient )
			{
				new MinimalHudEntity();
			}
		}
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

				var player = new Player();
				client.Pawn = player;
				player.Respawn();
			Host.AssertServer();
		}

		[ServerCmd("spawn_platform")]
		public static void SpawnPlatform()
		{
			Random rnd = new Random();
			int x = rnd.Next( 50, 151 );
			int y = rnd.Next( 50, 151 );
			int z = rnd.Next( 10, 51 );
			Prop platform = new Prop();
			platform.SetModel( "./platform.vmdl" );
			platform.AddCollisionLayer( CollisionLayer.Solid );
			platform.CollisionGroup = CollisionGroup.Prop;
			platform.Position = ConsoleSystem.Caller.Pawn.Position + new Vector3(x, y, z);
			platform.EnableTouch = true;
			platform.EnableHitboxes = true;
			platform.Spawn();
			Log.Info( platform.Position );
			platform.MoveType = MoveType.None;
			Log.Info( "touching" );
		}


		public override void BuildInput( InputBuilder inputBuilder )
		{
			
			inputBuilder.ViewAngles += inputBuilder.AnalogLook;
			inputBuilder.InputDirection += inputBuilder.AnalogMove;

			if ( inputBuilder.Pressed(InputButton.Jump) )
			{
				ConsoleSystem.Run( "spawn_platform" );
			}
		}





	}

}
