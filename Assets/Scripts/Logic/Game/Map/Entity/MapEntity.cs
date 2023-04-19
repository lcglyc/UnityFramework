using ECSModel;

public class MapEntity : Entity
{
    //这是那一关
    public int MapLevelID { get; set; }

    public override void Dispose()
    {
        if (this.IsDisposed)
        {
            return;
        }

        base.Dispose();
    }
}
