using System;
using ECSModel;
using UnityEngine;

namespace ECSModel
{
	public abstract class Component : Object, IDisposable
	{
		public long InstanceId { get; private set; }
		
		public static GameObject Global { get; } = GameObject.Find("/Global");
		
		public GameObject GameObject { get; protected set; }

		private bool isFromPool;

		public bool IsFromPool
		{
			get
			{
				return this.isFromPool;
			}
			set
			{
				this.isFromPool = value;

				if (!this.isFromPool)
				{
					return;
				}

				if (this.InstanceId == 0)
				{
					this.InstanceId = IdGenerater.GenerateInstanceId();
				}
			}
		}

		public bool IsDisposed
		{
			get
			{
				return this.InstanceId == 0;
			}
		}

		private Component parent;
		
		public Component Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
				if (this.parent == null)
				{
					this.GameObject.transform.SetParent(Global.transform, false);
					return;
				}

				if (this.GameObject != null && this.parent.GameObject != null)
				{
					this.GameObject.transform.SetParent(this.parent.GameObject.transform, false);
				}
			}
		}

		public T GetParent<T>() where T : Component
		{
			return this.Parent as T;
		}

		public Entity Entity
		{
			get
			{
				return this.Parent as Entity;
			}
		}
		
		protected Component()
		{
			this.InstanceId = IdGenerater.GenerateInstanceId();
			if (!this.GetType().IsDefined(typeof(HideInHierarchy), true))
			{
				this.GameObject = new GameObject();
				this.GameObject.name = this.GetType().Name;
				this.GameObject.layer = LayerNames.GetLayerInt(LayerNames.HIDDEN);
				this.GameObject.transform.SetParent(Global.transform, false);
				this.GameObject.AddComponent<ComponentView>().Component = this;
			}
		}


		public virtual void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			// 触发Destroy事件
			Game.EventSystem.Destroy(this);

			Game.EventSystem.Remove(this.InstanceId);
			
			this.InstanceId = 0;

			if (this.IsFromPool)
			{
				Game.ObjectPool.Recycle(this);
			}
			else
			{
				if (this.GameObject != null)
				{
					UnityEngine.Object.Destroy(this.GameObject);
				}
			}
		}

		public override void EndInit()
		{
			Game.EventSystem.Deserialize(this);
		}
	}
}