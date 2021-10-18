
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
			
		}

		[ServerCmd("spawn_platform")]
		public static void SpawnPlatform()
		{
			Random rnd = new Random();
			int x = rnd.Next( 50, 151 );
			int y = rnd.Next( 50, 151 );
			int z = rnd.Next( 0, 51 );
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

		[ServerCmd( "generate_map" )]
		public static void GenerateMap()
		{
			int a = 50;
			int b = 151;
			int c = 0;
			int d = 51;
			for ( int i = 0; i < 1000; i++ )
			{
				a = a + 100;
				b = b + 100;
				c = c + 100;
				d = d + 100;
				Random rnd = new Random();
				int x = rnd.Next( a, b );
				int y = rnd.Next( a, b );
				int z = rnd.Next( 0, 51 );
				Prop platform = new Prop();
				platform.SetModel( "./platform.vmdl" );
				platform.AddCollisionLayer( CollisionLayer.Solid );
				platform.CollisionGroup = CollisionGroup.Prop;
				platform.Position = new Vector3( x, y, z );
				platform.EnableTouch = true;
				platform.EnableHitboxes = true;
				platform.Spawn();
				Log.Info( platform.Position );
				platform.MoveType = MoveType.None;
				Log.Info( "touching" );
			}
		}
	}

}
