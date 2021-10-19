using System.Threading.Tasks;

namespace Sandbox
{
	/// <summary>
	/// A simple trigger volume that fires once and then removes itself.
	/// </summary>
	[Library( "parkour_trigger_start" )]
	[Hammer.Solid]
	public partial class TriggerStart : BaseTrigger
	{
		public override void Spawn()
		{
			base.Spawn();

			EnableTouchPersists = true;
		}

		public override void OnTouchStart( Entity other )
		{
			ConsoleSystem.Run( "generate_map " + other.Position.x + " " + other.Position.y + " " + other.Position.z );

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
