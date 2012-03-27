using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Collisions;
using System;
using Microsoft.Xna.Framework;
using FarseerGames.FarseerPhysics.Factories;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ProjectDaHooch
{
	//TODO: The body and geom dimensions should changed based upon the scale of the sprite. Probably should
	//		keep the transform data all in one class. SpriteAspect can still have its own scale, but should be
	//		added on top of the regular scaling done by OrientationAspect and BoxCollisionAspect.
	public class BoxCollisionAspect : OrientationAspect, IUpdateable
	{

		private static Dictionary<int, int> GeomId2EntityID = new Dictionary<int, int>();
		public static int FindEntityByGeomID(int id)
		{
			if (GeomId2EntityID.ContainsKey(id))
				return GeomId2EntityID[id];

			else return -1;
		}

		public override Rectangle Dimensions
		{
			get { return dimensions; }
			set 
			{
				this.dimensions = value;
				if (Loaded)
					InstantiatePhysicsObjects();
			}
		}

		public float Mass
		{
			get { return mass; }
			set { this.mass = value; }
		}
		public bool IsStatic
		{
			get
			{
				if (Loaded)
					return body.IsStatic;
				else
					return preIsStatic;
			}
			set
			{
				if (Loaded)
					body.IsStatic = value;
				else
					preIsStatic = value;
			}
		}
		public override Vector2 Position
		{
			get
			{
				if (Loaded)
					return geom.Position;
				else
					return prePosition;
			}
			set
			{
				if (Loaded)
				{
					body.ResetDynamics();
					body.Position = value;
				}
				else
					prePosition = value;
			}
		}

		public override float Rotation
		{
			get
			{
				if (Loaded)
					return body.Rotation;
				else
					return preRotation;
			}
			set
			{
				if (Loaded)
					body.Rotation = value;
				else
					preRotation = value;
			}
		}

		public int CollisionGroup
		{
			get
			{
				if (Loaded)
					return geom.CollisionGroup;
				else
					return preCollisionGroup;
			}
			set
			{
				if (Loaded)
					geom.CollisionGroup = value;
				else
					preCollisionGroup = value;
			}
		}

		[XmlIgnore]
        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
		public CollisionEventHandler OnCollision { get; set; }

        [XmlIgnore]
        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
		public Body body;
        [XmlIgnore]
        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
		public Geom geom;
		private IPhysicsService physicsService;
		private Rectangle dimensions = new Rectangle();
		private float mass;
		private Vector2 prePosition;
		private bool preIsStatic;
		private float preRotation;
		private int preCollisionGroup = 0;

		public BoxCollisionAspect()
		{
			EnabledChanged += OnEnabledChanged;
			UpdateOrderChanged += OnUpdateOrderChanged;
			this.enabled = false;
		}

		private void InstantiatePhysicsObjects()
		{
			Vector2 position;
			float rotation;
			bool isStatic;
			float momentOfInertia;
			int collisionGroup;
			if (Loaded)
			{
				momentOfInertia = body.MomentOfInertia;
				isStatic = this.IsStatic;
				position = this.Position;
				rotation = this.Rotation;
				collisionGroup = this.CollisionGroup;
				GeomId2EntityID.Remove(geom.Id);

				physicsService.Simulator.Remove(body);
				physicsService.Simulator.Remove(geom);

			}
			else
			{
				collisionGroup = preCollisionGroup;
				momentOfInertia = float.MaxValue;
				isStatic = this.preIsStatic;
				position = prePosition;
				rotation = preRotation;
			}
			body = BodyFactory.Instance.CreateRectangleBody(physicsService.Simulator, dimensions.Width, dimensions.Height, this.mass);
			body.Position = position;
			body.Rotation = rotation;
			body.IsStatic = isStatic;
			body.MomentOfInertia = momentOfInertia;
			geom = GeomFactory.Instance.CreateRectangleGeom(physicsService.Simulator, body, dimensions.Width, dimensions.Height);
			GeomId2EntityID.Add(geom.Id, OwnerID);
			geom.CollisionGroup = collisionGroup;

			if (OnCollision != null)
				geom.OnCollision = OnCollision;
		}

		protected override void InitSubclass()
		{
			this.physicsService = (IPhysicsService)Owner.Game.Services.GetService(typeof(IPhysicsService));
		}

		protected override void LoadSubclassContent()
		{
			if (dimensions.Width == 0)
				dimensions.Width = 1;
			if (dimensions.Height == 0)
				dimensions.Height = 1;
			InstantiatePhysicsObjects();			
		}

		public void Update(GameTime gameTime)
		{
			//TODO: Find a reason to call this, or remove the IUpdateable interface. It's disabled for now.
		}

		// TODO: Write Clone method for BoxCollisionAspect
		protected override IAspect CloneSubclass()
		{
			BoxCollisionAspect ba = new BoxCollisionAspect();
			
			ba.Position = this.Position;
			ba.Rotation = this.Rotation;
			ba.OnCollision = this.OnCollision;
			ba.Mass = this.mass;
			ba.IsStatic = this.IsStatic;
			ba.Dimensions = this.dimensions;
			ba.Enabled = this.Enabled;
			ba.CollisionGroup = this.CollisionGroup;

			return ba;
		}

		#region IUpdateable Members

		private int updateOrder;
		private bool enabled;

		public bool Enabled
		{
			get{ return enabled; }
			set { enabled = value; EnabledChanged(this, new EventArgs()); }
		}

		public event System.EventHandler EnabledChanged;
		public void OnEnabledChanged(object sender, EventArgs e) { }

		public int UpdateOrder
		{
			get { return updateOrder; }
			set { updateOrder = value; UpdateOrderChanged(this, new EventArgs()); }
		}

		public event System.EventHandler UpdateOrderChanged;
		public void OnUpdateOrderChanged(object sender, EventArgs e) { }

		#endregion
	}
}
