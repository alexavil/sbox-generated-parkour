using System.Threading.Tasks;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Sandbox
{
	/// <summary>
	/// A simple trigger volume that fires once and then removes itself.
	/// </summary>
	[Library( "parkour_trigger_fail" )]
	[Hammer.Solid]
	public partial class TriggerFail : BaseTrigger
	{
		public override void Spawn()
		{
			base.Spawn();

			EnableTouchPersists = true;
		}

		public override void OnTouchStart( Entity other )
		{
			ConsoleSystem.Run( "reset_map" );

			var Targetent = Entity.FindByName( "spawn" );

			if ( Targetent != null )
			{
				other.Transform = Targetent.Transform;
				other.Position = Targetent.Position;

			}
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
