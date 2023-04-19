using ECSModel;
public static class MapFactory
{
    public static async ECSTask Create( int levelID )
    {
        long id = RandomHelper.RandInt64();
        MapEntity map = ComponentFactory.CreateWithId<MapEntity>(id);
        //当前有且只有一张地图
        MapComponent.Inst.CurMap = map;

        InGameDataCom gameData = map.AddComponent<InGameDataCom>();
        gameData.Init(levelID);

        JsonLibComponent jsonlib = Game.Scene.GetComponent<JsonLibComponent>();
        string gameeffect = jsonlib.GetGameEffect(levelID);
        if (!string.IsNullOrEmpty(gameeffect))
        {
            MapEffectCompoent mapEffect = map.AddComponent<MapEffectCompoent>();
            await mapEffect.LoadLevelEffectPrefabs("boss5_effect.unity3d","Boss5_Effect");
        }
       
        // 加载场景
        await WaveFactory.Create(levelID,gameData);
        // 加载lua 脚本

        // 创建多个球
        AddBallCom(map);
    }

    // 场景里会有多个球
    public static void AddBallCom(MapEntity map)
    {
        Ball ball = BallComponent.Instance.CurBall;
        BallAttributeCom attriCom = ball.GetComponent<BallAttributeCom>();
        // 这里去创建其他的球，继承主球的 avatar和 speed
        BallMoveCom move = ball.AddComponent<BallMoveCom>();
        move.Init(attriCom.BallSpd);

        BallSplitCom splitcom = map.GetComponent<BallSplitCom>();
        if (splitcom == null) splitcom = map.AddComponent<BallSplitCom>();
        splitcom.Init(attriCom.BallNumber, ball.ConfigID, attriCom.BallSpd, ball).Coroutine();
    }

}
