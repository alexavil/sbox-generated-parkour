using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;

namespace ParkourGame
{
	public class LoopCompletePopUp : Panel
	{
		public Label Label;

		public LoopCompletePopUp()
		{
			SetTemplate( "./UI/loopcomplete.html" );
		}
	}
}
