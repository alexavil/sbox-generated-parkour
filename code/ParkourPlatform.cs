using System;
using System.Threading.Tasks;

namespace Sandbox
{
	/// <summary>
	/// A prop that physically simulates as a single rigid body. It can be constrained to other physics objects using hinges
	/// or other constraints. It can also be configured to break when it takes enough damage.
	/// Note that the health of the object will be overridden by the health inside the model, to ensure consistent health game-wide.
	/// If the model used by the prop is configured to be used as a prop_dynamic (i.e. it should not be physically simulated) then it CANNOT be
	/// used as a prop_physics. Upon level load it will display a warning in the console and remove itself. Use a prop_dynamic instead.
	/// </summary>
	[Library( "parkour_platform" )]
	[Hammer.Model]
	[Hammer.RenderFields]
	public partial class ParkourPlatform : BasePhysics
	{
		[ServerVar]
		public static bool debug_prop_explosion { get; set; } = false;

		protected enum PropCollisionGroups
		{
			UNUSED = -1,
			COLLISION_GROUP_ALWAYS = CollisionGroup.Always,
			COLLISION_GROUP_NONPHYSICAL = CollisionGroup.Never,
			COLLISION_GROUP_DEFAULT = CollisionGroup.Default,
			COLLISION_GROUP_DEBRIS = CollisionGroup.Debris,
			COLLISION_GROUP_WEAPON = CollisionGroup.Weapon,
		};

		[Property]
		protected PropCollisionGroups CollisionGroupOverride { get; set; } = PropCollisionGroups.UNUSED;

		public override void Spawn()
		{
			base.Spawn();

			MoveType = MoveType.Physics;
			CollisionGroup = CollisionGroup.Interactive;
			PhysicsEnabled = true;
			UsePhysicsCollision = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			if ( CollisionGroupOverride != PropCollisionGroups.UNUSED )
			{
				CollisionGroup = (CollisionGroup)CollisionGroupOverride;
			}
		}

		public override void ClientSpawn()
		{
			base.ClientSpawn();

			SpawnParticles();
		}

		private void SpawnParticles()
		{
			var model = GetModel();
			if ( model == null || model.IsError )
				return;

			var particleList = model.GetParticles();
			if ( particleList == null || particleList.Length <= 0 )
				return;

			foreach ( var particleData in particleList )
			{
				Particles.Create( particleData.Name, this, particleData.AttachmentPoint );
			}
		}

		public override void OnNewModel( Model model )
		{
			base.OnNewModel( model );

			if ( IsServer )
			{
				UpdatePropData( model );
			}
		}

		protected virtual void UpdatePropData( Model model )
		{
			Host.AssertServer();

			var propInfo = model.GetPropData();
			Health = propInfo.Health;

			//
			// If health is unset, set it to -1 - which means it cannot be destroyed
			//
			if ( Health <= 0 )
				Health = -1;
		}

		DamageInfo LastDamage;
		int explosionCasualties = 0;

		/// <summary>
		/// Fired when the entity gets damaged
		/// </summary>
		protected Output OnDamaged { get; set; }

		public override void TakeDamage( DamageInfo info )
		{
			if ( Invulnerable > 0 )
			{
				// We still want to apply forces
				ApplyDamageForces( info );

				return;
			}

			LastDamage = info;

			base.TakeDamage( info );

			OnDamaged.Fire( this );
		}

		public override void OnKilled()
		{
			if ( LifeState != LifeState.Alive )
				return;

			if ( LastDamage.Flags.HasFlag( DamageFlags.PhysicsImpact ) )
			{
				Velocity = lastCollision.PreVelocity;
			}

			var result = new Breakables.Result();
			result.Params.DamagePositon = LastDamage.Position;
			Breakables.Break( this, result );

			if ( LastDamage.Flags.HasFlag( DamageFlags.Blast ) )
			{
				foreach ( var prop in result.Props )
				{
					if ( !prop.IsValid() )
						continue;

					var body = prop.PhysicsBody;
					if ( !body.IsValid() )
						continue;

					body.ApplyImpulseAt( LastDamage.Position, LastDamage.Force * 25.0f );
				}
			}

			if ( HasExplosionBehavior() )
			{
				if ( LastDamage.Flags.HasFlag( DamageFlags.Blast ) && LastDamage.Attacker is ParkourPlatform prop )
				{
					prop.explosionCasualties++;
					_ = ExplodeAsync( 0.1f * prop.explosionCasualties );

					return;
				}
				else
				{
					OnExplosion();
				}
			}

			base.OnKilled();
		}

		CollisionEventData lastCollision;

		/// <summary>
		/// This prop won't be able to be damaged for this amount of time
		/// </summary>
		public RealTimeUntil Invulnerable { get; set; }

		protected override void OnPhysicsCollision( CollisionEventData eventData )
		{
			ParkourPlatform model = new ParkourPlatform();
			model.SetModel( "./platform.vmdl" );
			model.AddCollisionLayer( CollisionLayer.Solid );
			model.Position = eventData.Entity.Position + new Vector3( 100, 0, 0 );
			model.EnableTouch = true;
			model.Spawn();
			Log.Info( model.Position );
			model.MoveType = MoveType.None;
		}

		public async Task ExplodeAsync( float fTime )
		{
			if ( LifeState != LifeState.Alive )
				return;

			LifeState = LifeState.Dead;

			await Task.DelaySeconds( fTime );
			OnExplosion();

			Delete();
		}

		private bool HasExplosionBehavior()
		{
			var model = GetModel();
			if ( model == null || model.IsError )
				return false;

			return model.HasExplosionBehavior();
		}

		private void OnExplosion()
		{
			var model = GetModel();
			if ( model == null || model.IsError )
				return;

			if ( !PhysicsBody.IsValid() )
				return;

			if ( !model.HasExplosionBehavior() )
				return;

			var explosionBehavior = model.GetExplosionBehavior();

			if ( !string.IsNullOrWhiteSpace( explosionBehavior.Sound ) )
			{
				Sound.FromWorld( explosionBehavior.Sound, PhysicsBody.MassCenter );
			}
			else
			{
				// TODO: Replace with something else
				Sound.FromWorld( "rust_pumpshotgun.shootdouble", PhysicsBody.MassCenter );
			}

			if ( !string.IsNullOrWhiteSpace( explosionBehavior.Effect ) )
			{
				Particles.Create( explosionBehavior.Effect, PhysicsBody.MassCenter );
			}
			else
			{
				Particles.Create( "particles/explosion/barrel_explosion/explosion_barrel.vpcf", PhysicsBody.MassCenter );
			}

			if ( explosionBehavior.Radius > 0.0f )
			{
				var sourcePos = PhysicsBody.MassCenter;
				var overlaps = Physics.GetEntitiesInSphere( sourcePos, explosionBehavior.Radius );

				if ( debug_prop_explosion )
					DebugOverlay.Sphere( sourcePos, explosionBehavior.Radius, Color.Orange, true, 5 );

				foreach ( var overlap in overlaps )
				{
					if ( overlap is not ModelEntity ent || !ent.IsValid() )
						continue;

					if ( ent.LifeState != LifeState.Alive )
						continue;

					if ( !ent.PhysicsBody.IsValid() )
						continue;

					if ( ent.IsWorld )
						continue;

					var targetPos = ent.PhysicsBody.MassCenter;

					var dist = Vector3.DistanceBetween( sourcePos, targetPos );
					if ( dist > explosionBehavior.Radius )
						continue;

					var tr = Trace.Ray( sourcePos, targetPos )
						.Ignore( this )
						.WorldOnly()
						.Run();

					if ( tr.Fraction < 0.95f )
					{
						if ( debug_prop_explosion )
							DebugOverlay.Line( sourcePos, tr.EndPos, Color.Red, 5, true );

						continue;
					}

					if ( debug_prop_explosion )
						DebugOverlay.Line( sourcePos, targetPos, 5, true );

					var distanceMul = 1.0f - Math.Clamp( dist / explosionBehavior.Radius, 0.0f, 1.0f );
					var damage = explosionBehavior.Damage * distanceMul;
					var force = (explosionBehavior.Force * distanceMul) * ent.PhysicsBody.Mass;
					var forceDir = (targetPos - sourcePos).Normal;

					ent.TakeDamage( DamageInfo.Explosion( sourcePos, forceDir * force, damage )
						.WithAttacker( this ) );
				}
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		[Event.Physics.PostStep]
		public void OnPostPhysicsStep()
		{
			if ( !this.IsValid() )
				return;

			Liquid?.Update();
		}
	}
}
