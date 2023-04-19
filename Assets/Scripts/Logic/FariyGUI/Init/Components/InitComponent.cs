using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FairyGUI;
using ECSModel;
using MonogolyConfig;

[ObjectSystem]
public class InitComponentSystem : AwakeSystem<InitComponent>
{
    public override void Awake(InitComponent self)
    {
        self.Awake(self);
    }
}
public class InitComponent : Component
{
    GTextField mTips;
    TimerComponent timers;
    string mTipsMsg;
    public void Awake(InitComponent self)
    {
        timers = Game.Scene.GetComponent<TimerComponent>();
        FUI panel = self.GetParent<FUI>();
        panel.GObject.asCom.MakeFullScreen();
        mTips = panel.Get("tips").GObject.asTextField;
        mTipsMsg = mTips.text;
        RunTips();
    }

    async UniTaskVoid RunTips()
    {
        string tmp = string.Format(mTipsMsg, 50);
        mTips.text = tmp;

        tmp = "正在初始化人物";
        mTips.text = tmp;
        Game.EventSystem.Run(EventIdType.InitPlayer);
        await timers.WaitAsync(100);

        tmp = "正在初始化主界面";
        mTips.text = tmp;
        await LoadMainGame();

        tmp = "正在加载 游戏组件";
        mTips.text = tmp;
        await LoadBall();
        await LoadRacket();
        await loadGm();

        tmp = "正在进入游戏";
        await timers.WaitAsync(100);
        Game.EventSystem.Run(EventIdType.InitGameFinish);
    }

    async UniTask LoadMainGame()
    {
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        FUI ui = await MainGameFactory.Create();
        ui.Visible = false;
        fuiComponent.Add(ui);
    }

    async UniTask loadGm()
    {
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        FUI ui = await DebugFactory.Create();
        FUI battle = await DebugBattleFactory.Create();
        fuiComponent.Add(ui);
        fuiComponent.Add(battle);
        ui.Visible = false;
        battle.Visible = false;
    }

    async UniTask LoadBall()
    {
        var ballBaseData = MonogolyConfig.BallBaseDataManager.GetInstance().GetConfigDic();
        Dictionary<long, BallAttributeCom> mSerializeds = GetSerializedData();
        if (mSerializeds != null && mSerializeds.Count != 0)
        {
            await IniiBallFromDB(mSerializeds);
        }
        else
        {
            await InitBallDataFromConfig(ballBaseData);
        }
    }


    Dictionary<long, BallAttributeCom> GetSerializedData()
    {
        SerializationComponent serializationComponent = Game.Scene.GetComponent<SerializationComponent>();
        return serializationComponent.GetSerializeBallAttributeCom();
    }

    async UniTask IniiBallFromDB(Dictionary<long, BallAttributeCom> mSerializeds)
    {
        foreach (var VARIABLE in mSerializeds.Keys)
        {
            BallAttributeCom tmpData = mSerializeds[VARIABLE];
            Ball ball = await BallFactory.Create(VARIABLE, tmpData.ConfigID, tmpData);
            if (tmpData.ConfigID == 10001)
            {
                BallComponent.Instance.CurBall = ball;
                ball.GetComponent<BallAttributeCom>().IsUnlock = true;
                ball.Visable = true;
            }
            else
            {
                ball.Visable = false;
            }

            BallComponent.Instance.Add(ball);
        }
    }

    async UniTask InitBallDataFromConfig(Dictionary<int, BallBaseData> ballBaseData)
    {
        int index = 0;
        foreach (var data in ballBaseData.Values)
        {
            if (data.ID <= 0)
            {
                Log.Error("ball ID = " + data.ID);
                continue;
            }
            long id = RandomHelper.RandInt64();
            Ball ball = await BallFactory.Create(id, data.ID);

            if (data.ID == 10001)
            {
                BallComponent.Instance.CurBall = ball;
                ball.GetComponent<BallAttributeCom>().IsUnlock = true;
            }

            ball.Visable = false;
            ball.GetComponent<BallPostionCom>().SetBallStartPostion(data.DefalutScale);
            BallComponent.Instance.Add(ball);
            index++;
        }
    }


    async UniTask LoadRacket()
    {
        Dictionary<long, RacketAttributeCom> rackets = GetSerializedRacketData();
        if (rackets == null || rackets.Count == 0)
        {
            await CreateRacketFromConfig();
        }
        else
        {
            await CreateRacketFromSerialize(rackets);
        }


    }

    async UniTask CreateRacketFromSerialize(Dictionary<long, RacketAttributeCom> rackets)
    {
        foreach (var VARIABLE in rackets.Keys)
        {
            RacketAttributeCom tmpData = rackets[VARIABLE];
            Racket racket = await RacketFactory.Create(VARIABLE, tmpData.ConfigID, tmpData);
            if (tmpData.ConfigID == 20001)
            {
                RacketComponent.Instance.CurRacket = racket;
                racket.GetComponent<RacketAttributeCom>().IsUnlock = true;
            }
            racket.Visible = false;
        }
    }

    Dictionary<long, RacketAttributeCom> GetSerializedRacketData()
    {
        SerializationComponent serializationComponent = Game.Scene.GetComponent<SerializationComponent>();
        return serializationComponent.GetSerializeRacketAttributeCom();
    }

    async UniTask CreateRacketFromConfig()
    {
        var boardBaseData = MonogolyConfig.BoardBaseDataManager.GetInstance().GetConfigDic();
        foreach (var data in boardBaseData.Values)
        {
            if (data.ID <= 0)
            {
                Log.Error("racket ID = " + data.ID.ToString());
                continue;
            }
            long id = RandomHelper.RandInt64();
            Racket racket = await RacketFactory.Create(id, data.ID);

            if (data.ID == 20001)
            {
                RacketComponent.Instance.CurRacket = racket;
                racket.GetComponent<RacketAttributeCom>().IsUnlock = true;
            }
            racket.Visible = false;
        }

    }



    // 这里暂时没啥要扔掉的
    public override void Dispose()
    {
        base.Dispose();
    }

}