using ECSModel;

[Event(EventIdType.ChangePlayer)]
public class ChangePlayerSystem : AEvent<long>
{
    public override void Run(long id)
    {
        // 要切换的id，或者新建的一个id
    }
}
