using ECSModel;
using UnityEngine;

[ObjectSystem]
public class WaveEntitySystem: AwakeSystem<WaveEntity, GameObject>
{
    public override void Awake(WaveEntity self, GameObject a)
    {
        self.Awake(a);
    }
}

public class WaveEntity : Entity
{
    public int WaveId { get; set; }
    public int BaseLevle { get; set; }

    public void Awake( GameObject go)
    {
        this.GameObject = go;
        this.GameObject.AddComponent<ComponentView>().Component= this;
    }

    public Vector3 Position
    {
        get { return this.GameObject.transform.position; }
        set { this.GameObject.transform.position = value; }
    }

    public override void Dispose()
    {
        MonoBehaviour.Destroy(this.GameObject);
        base.Dispose();
    }

}
