using Sandbox;
using System;

namespace ParkourGame
{


	public partial class ParkourGame : Sandbox.Game
	{
		public ParkourGame()
		{
			new ParkourHudEntity();
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
			for ( int i = 0; i < 1000; i++ )
			{
				a = a + 150;
				b = b + 100;
				c = c + 100;
				d = d + 100;
				Random rnd = new Random();
				int x = a;
				int y = rnd.Next( 50, 251 );
				int z = rnd.Next( 10, 51 );
				Prop platform = new Prop();
				platform.SetModel( "./platform_small.vmdl" );
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
			var allEnts = Prop.FindAllByName("platform");
			foreach ( var ent in allEnts )
			{
				ent.Delete();
			}

		}
		public override void BuildInput( InputBuilder input )
		{
			base.BuildInput( input );
			if ( input.Pressed(InputButton.Jump) )
			{
				int jumps = FileSystem.Data.ReadAllText( "./jumps.txt" ).ToInt();
				int newjumps = jumps + 1;
				FileSystem.Data.WriteAllText( "./jumps.txt", newjumps.ToString() );
			}

		}
	}

}
