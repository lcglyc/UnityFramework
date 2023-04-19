using ECSModel;
using System.Collections.Generic;
using System.Linq;

[ObjectSystem]
class WaveComponentAwakeSystem : AwakeSystem<WaveComponent>
{
    public override void Awake(WaveComponent self)
    {
        self.Awake();
    }
}
public class WaveComponent : Component
{
    public static WaveComponent Instance { get;  private set; }

    private readonly Dictionary<long, WaveEntity> waves = new Dictionary<long, WaveEntity>();

    public void Awake()
    {
        Instance = this;
    }

    public void Add(WaveEntity wave)
    {
        this.waves.Add(wave.Id, wave);
        wave.Parent = this;
    }

    public void Remove( long id)
    {
        waves.Remove(id);
    }

    public void RemoveAndDiopose( long id)
    {
        GetEnityID(id).Dispose();
        waves.Remove(id);
    }

    public WaveEntity GetEnityID( long id)
    {
        WaveEntity player;
        this.waves.TryGetValue(id, out player);
        return player;
    }

    public WaveEntity GetByWaveID( int id)
    {
        foreach(  WaveEntity  enitiy in waves.Values )
        {
            if( enitiy.WaveId == id)
                return enitiy;
        }

        return null;
    }

    public int Count
    {
        get
        {
            return this.waves.Count;
        }
    }

    public WaveEntity[] GetAll()
    {
        return this.waves.Values.ToArray();
    }

    public override void Dispose()
    {
        if (this.IsDisposed)
        {
            return;
        }

        base.Dispose();

        foreach (WaveEntity player in this.waves.Values)
        {
            player.Dispose();
        }
        waves.Clear();
        Instance = null;
    }

}
