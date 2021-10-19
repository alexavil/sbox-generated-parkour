
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

		[ServerCmd( "generate_map" )]
		public static void GenerateMap( float startx, float starty, float startz )
		{
			int a = 50;
			int b = 151;
			int c = 0;
			int d = 51;
			int rng;
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
				rng = rnd.Next( 1, 3 );
				Prop platform = new Prop();
				switch ( rng )
				{
					case 1:
						platform.SetModel( "./platform.vmdl" );
						break;
					case 2:
						platform.SetModel( "./platform_small.vmdl" );
						break;
				}
				platform.AddCollisionLayer( CollisionLayer.Solid );
				platform.CollisionGroup = CollisionGroup.Prop;
				platform.Position = new Vector3( startx, starty, startz ) + new Vector3( x, y, z );
				platform.EnableTouch = true;
				platform.EnableHitboxes = true;
				platform.Name = "platform";
				platform.Spawn();
				Log.Info( platform.Position );
				platform.MoveType = MoveType.None;
				Log.Info( "touching" );
			}
		}

		[ServerCmd("reset_map")]
		public static void ResetMap()
		{
			var allEnts = Prop.FindAllByName("platform");
			foreach ( var ent in allEnts )
			{
				ent.Delete();
			}

		}
	}

}
