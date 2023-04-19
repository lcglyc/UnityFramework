using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ECSModel
{
    [ObjectSystem]
    public class BulletAwakeSystem : AwakeSystem<Bullet, GameObject>
    {
        public override void Awake(Bullet self, GameObject gameObject)
        {
            self.Awake(gameObject);
        }
    }
    public sealed class Bullet : Entity
    {
        public void Awake(GameObject go)
        {
            this.GameObject = go;
            this.GameObject.AddComponent<ComponentView>().Component = this;
        }

        public Vector3 Position
        {
            get
            {
                return GameObject.transform.position;
            }

            set
            {
                GameObject.transform.position = value;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.GameObject != null)
            {
                MonoBehaviour.Destroy(this.GameObject);
                this.GameObject = null;
            }

            base.Dispose();
        }
    }
}
