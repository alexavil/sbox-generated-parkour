using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace ParkourGame
{

	public partial class ParkourHudEntity : Sandbox.HudEntity<RootPanel>
	{
		public ParkourHudEntity()
		{
			if (IsClient)
			{
				RootPanel.StyleSheet.Load( "./UI/score.scss" );
				RootPanel.AllowChildSelection = true;
				RootPanel.AddChild<Score>();
			}
		}

	}

}
