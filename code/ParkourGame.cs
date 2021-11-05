using Sandbox;
using System;

namespace ParkourGame
{


	public partial class ParkourGame : Sandbox.Game
	{
		public static WIPHudEntity alert;
		public ParkourGame()
		{
			new ParkourHudEntity();
			alert = new WIPHudEntity();
			FileSystem.Data.WriteAllText( "./attempts.txt", "0" );
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

				var player = new Player();
				client.Pawn = player;
				player.Respawn();
		}
		public override void DoPlayerDevCam( Client player )
		{
		}
		public override void DoPlayerNoclip( Client player )
		{
		}

		[ServerCmd( "generate_map" )]
		public static void GenerateMap( float startx, float starty, float startz )
		{
			int a = 30;
			int b = 251;
			int c = 0;
			int d = 51;
			for ( int i = 0; i < 2000; i++ )
			{
				a = a + 150;
				b = b + 100;
				c = c + 100;
				d = d + 100;
				Random rnd = new Random();
				int x = a;
				int y = rnd.Next( 0, 201 );
				int z = rnd.Next( 10, 51 );
				Prop platform = new Prop();
				platform.SetModel( "./models/platform_small.vmdl" );
				platform.AddCollisionLayer( CollisionLayer.Solid );
				platform.CollisionGroup = CollisionGroup.Prop;
				platform.Position = new Vector3( startx, starty, startz ) + new Vector3( x, y, z );
				platform.EnableTouch = true;
				platform.EnableHitboxes = true;
				platform.Name = "platform";
				platform.Spawn();
				Log.Info( "Spawned platform at " + platform.Position );
				platform.MoveType = MoveType.None;
				FileSystem.Data.WriteAllText( "./jumps.txt", "0" );
			}
		}


		[ServerCmd("reset_map")]
		public static void ResetMap()
		{
			int attempts = FileSystem.Data.ReadAllText( "./attempts.txt" ).ToInt();
			int newattempts = attempts + 1;
			FileSystem.Data.WriteAllText( "./attempts.txt", newattempts.ToString() );
			var allEnts = Prop.FindAllByName("platform");
			foreach ( var ent in allEnts )
			{
				ent.Delete();

			}

		}

	}

}