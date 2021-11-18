using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox;
using System.Threading.Tasks;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace ParkourGame
{

	public partial class WIPHudEntity : Sandbox.HudEntity<RootPanel>
	{
		public WIPHudEntity()
		{
			if (IsClient)
			{
				RootPanel.AllowChildSelection = true;
				var alert = RootPanel.AddChild<AlertPopUp>();
				delete();
				async Task<string> delete() {
					await Task.DelaySeconds( 5 );
					alert.Delete();
					RootPanel.StyleSheet.Load("./score.scss");
					return "true";
				}
				
				
			}
		}

	}

}
