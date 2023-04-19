using ECSModel;

[Event(EventIdType.InitPlayer)]
public class InitPlayerSystem : AEvent
{
    public override void Run()
    {
        long id = RandomHelper.RandInt64();
        SerializationComponent serialize = Game.Scene.GetComponent<SerializationComponent>();
        PlayerFactory.Create(id);
    }
}
