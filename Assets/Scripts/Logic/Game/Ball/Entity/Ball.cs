using UnityEngine;
namespace ECSModel
{

    [ObjectSystem]
    public class UnitAwakeSystem : AwakeSystem<Ball, GameObject>
    {
        public override void Awake(Ball self, GameObject gameObject)
        {
            self.Awake(gameObject);
        }
    }
    public sealed class Ball : Entity
    {
        public long UnitId { get; set; }
        public string Name { get; set; }
        public int ConfigID { get; set; }

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

        public Vector3 LocalPosition
        {
            get
            {
                return GameObject.transform.localPosition;
            }

            set
            {
                GameObject.transform.localPosition = value;
            }
        }

        public Vector3 LocalScale
        {
            get
            {
                return GameObject.transform.localScale;
            }
            set
            {
                GameObject.transform.localScale = value;
            }
        }

        public bool Visable
        {
            get
            {
                return GameObject.activeSelf;
            }
            set { GameObject.SetActive(value); }
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