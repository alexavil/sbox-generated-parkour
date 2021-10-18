using System.Threading.Tasks;

namespace Sandbox
{
	/// <summary>
	/// A simple trigger volume that fires once and then removes itself.
	/// </summary>
	[Library( "parkour_trigger_next" )]
	[Hammer.Solid]
	public partial class TriggerNext : BaseTrigger
	{
		public override void Spawn()
		{
			base.Spawn();

			EnableTouchPersists = true;
		}

		public override void OnTouchStart( Entity other )
		{
			TriggerNext next = new TriggerNext();
			Prop model = new Prop();
			model.SetModel( "./platform.vmdl" );
			model.AddCollisionLayer( CollisionLayer.Solid );
			model.Position = other.Position + new Vector3( 0, 50, 0 );
			model.Spawn();
			model.MoveType = MoveType.None;
			next.Position = model.Position;
			next.Scale = 0.5f;
			Log.Info( next.Position );
			Log.Info( model.Position );
			next.Spawn();
		}

		/// <summary>
		/// Called once at least a single entity that passes filters is touching this trigger, just before this trigger getting deleted
		/// </summary>
		protected Output OnTrigger { get; set; }

		public virtual void OnTriggered( Entity other )
		{

		}
	}
}
