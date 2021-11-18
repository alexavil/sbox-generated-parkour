using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox;
using System.Threading.Tasks;

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
				var alert = RootPanel.AddChild<AlertPopUp>();
				delete();
				async Task<string> delete()
				{
					await Task.DelaySeconds( 5 );
					alert.Delete();
					RootPanel.StyleSheet.Load( "./UI/score.scss" );
					return "true";
				}
			}
		}

	}

}
