using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ParkourGame
{
	public class Score : Panel
	{
		public Label Label;

		public Score()
		{
			Label = Add.Label();
		}

		public override void Tick()
		{
			var player = Local.Pawn;
			if ( player == null ) return;

			Label.Text = "High Score: " + FileSystem.Data.ReadAllText( "./highscore.txt" ) + "\nJumps: " + FileSystem.Data.ReadAllText( "./jumps.txt" ) + "\nAttempts: " + FileSystem.Data.ReadAllText( "./attempts.txt" );
		}
	}
}
