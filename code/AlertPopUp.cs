using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;

namespace ParkourGame
{
	public class AlertPopUp : Panel
	{
		public Label Label;
		public Button button;

		public AlertPopUp()
		{
			SetTemplate( "./alert.html" );
		}
	}
}
