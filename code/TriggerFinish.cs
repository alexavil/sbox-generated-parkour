using System.Threading.Tasks;

namespace Sandbox
{
	/// <summary>
	/// A simple trigger volume that fires once and then removes itself.
	/// </summary>
	[Library( "parkour_trigger_finish" )]
	[Hammer.Solid]
	public partial class TriggerFinish : BaseTrigger
	{
		public static ParkourGame.WinHud alert;
		public override void Spawn()
		{
			base.Spawn();

			EnableTouchPersists = true;
		}

		public override void OnTouchStart( Entity other )
		{
			ConsoleSystem.Run( "reset_map" );

			string diff = FileSystem.Data.ReadAllText( "./difficulty.txt" );

			int completed = FileSystem.Data.ReadAllText( "./completed_" + diff + ".txt" ).ToInt();
			int newcompleted = completed + 1;
			FileSystem.Data.WriteAllText( "./completed_" + diff + ".txt", newcompleted.ToString() );

			var Targetent = Entity.FindByName( "spawn" );

			if ( Targetent != null )
			{
				other.Transform = Targetent.Transform;
				other.Position = Targetent.Position;

			}
			alert = new ParkourGame.WinHud();

		}

		/// <summary>
		/// Called once at least a single entity that passes filters is touching this trigger, just before this trigger getting deleted
		/// </summary>
		protected Output OnTrigger { get; set; }

		public virtual void OnTriggered( Entity other )
		{
			OnTrigger.Fire( other );
		}
	}
}
