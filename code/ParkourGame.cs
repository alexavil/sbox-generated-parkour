using Sandbox;
using System;

namespace ParkourGame
{


	public partial class ParkourGame : Sandbox.Game
	{

		public ParkourGame()
		{
			new ParkourHudEntity();

			FileSystem.Data.WriteAllText( "./completed_easy.txt", FileSystem.Data.ReadAllText( "./completed_easy.txt" ) );
			FileSystem.Data.WriteAllText( "./completed_normal.txt", FileSystem.Data.ReadAllText( "./completed_normal.txt" ) );
			FileSystem.Data.WriteAllText( "./completed_hard.txt", FileSystem.Data.ReadAllText( "./completed_hard.txt" ) );
			FileSystem.Data.WriteAllText( "./difficulty.txt", "normal" );
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
			int a = 30;
			int b = 251;
			int c = 0;
			int d = 51;
			for ( int i = 0; i < 216; i++ )
			{
				a = a + 150;
				b = b + 100;
				c = c + 100;
				d = d + 100;
				Random rnd = new Random();
				int x = a;
				int y = rnd.Next( 0, 201 );
				int z = rnd.Next( 10, 51 );
				int rng = rnd.Next( 1, 3 );
				Prop platform = new Prop();
				void generatenormal()
				{
					if ( rng == 1 ) platform.SetModel( "./models/platform_small.vmdl" );
					if ( rng == 2 ) platform.SetModel( "./models/platform.vmdl" );
				}
				string difficulty = FileSystem.Data.ReadAllText( "./difficulty.txt" );
				switch (difficulty) {

					case "easy":
						platform.SetModel( "./models/platform.vmdl" );
						break;

					case "normal":
						generatenormal();
						break;

					case "hard":
						platform.SetModel( "./models/platform_small.vmdl" );
						break;

				}

				platform.AddCollisionLayer( CollisionLayer.Solid );
				platform.CollisionGroup = CollisionGroup.Prop;
				platform.Position = new Vector3( startx, starty, startz ) - new Vector3( x, y, z );
				platform.EnableTouch = true;
				platform.EnableHitboxes = true;
				platform.Name = "platform";
				platform.Spawn();
				Logger logger = new Logger("platfominfo");
				logger.Info( "Spawned platform at " + platform.Position );
				platform.MoveType = MoveType.None;
				FileSystem.Data.WriteAllText( "./jumps.txt", "0" );
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

		[ServerCmd( "setdifficulty" )]
		public static void SetDifficulty( string diff )
		{
			ConsoleSystem.Run( "reset_map" );
			FileSystem.Data.WriteAllText( "./difficulty.txt", diff );
			ConsoleSystem.Run( "kill" );
		}

	}

}
