using ECSModel;

[ObjectSystem]
class MapComponentSystem : AwakeSystem<MapComponent>
{
    public override void Awake(MapComponent self)
    {
        self.Awake();
    }
}

public class MapComponent : Component
{
    public static MapComponent Inst;
    public void Awake()
    {
        Inst = this;
    }

    private MapEntity map;
    public MapEntity CurMap
    {
        get => map;
        set
        {
            map = value;
            map.Parent = this;
        }
    }

    public override void Dispose()
    {
        map = null;
        base.Dispose();
    }
}
