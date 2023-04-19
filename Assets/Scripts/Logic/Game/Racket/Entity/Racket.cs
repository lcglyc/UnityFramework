using UnityEngine;

namespace ECSModel
{
    [ObjectSystem]
    public class RacketAwakeSystem : AwakeSystem<Racket, GameObject>
    {

        public override void Awake(Racket self, GameObject gameObject)
        {
            self.Awake(gameObject);
        }
    }

    public sealed class Racket : Entity
    {

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

        public float NineSliceScale
        {
            get
            {
                return GameObject.GetComponent<SpriteRenderer>().size.x;
            }
            set
            {
                var size = GameObject.GetComponent<SpriteRenderer>().size;
                GameObject.GetComponent<SpriteRenderer>().size = new Vector2(value, size.y);
            }
        }


        public Vector3 StartPosition
        {
            get
            {
                GetComponent<RacketMoveCom>().MoveToStartPostion();
                return GameObject.transform.position;
            }
        }
        public bool Visible
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

            base.Dispose();
        }
    }
}