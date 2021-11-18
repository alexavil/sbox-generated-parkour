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


			if ( Input.Down( InputButton.Score ) )
			{
				Label.Text = "";
				SetTemplate( "./UI/menu.html" );
			}
			else
			{
				SetTemplate( "" );
				string diff = FileSystem.Data.ReadAllText( "./difficulty.txt" );

				Label.Text = "Loops Completed: " + FileSystem.Data.ReadAllText( "./completed_" + diff + ".txt" ) + "\nJumps: " + FileSystem.Data.ReadAllText( "./jumps.txt" ) + "\nDifficulty: " + diff;
			}

			
		}
	}
}
