using ECSModel;
using System.Collections.Generic;

[Event(EventIdType.InitBattle)]
public class StartBattleSystem : AEvent
{
    public override void Run()
    {
        RunStartBattle().Coroutine();
    }

    async ECSVoid RunStartBattle()
    {
        // game  Start 动效
        Game.EventSystem.Run<bool>(EventIdType.UpdateMainGamePanelVisable, false);
        await Game.Scene.GetComponent<TimerComponent>().WaitAsync(650);

        CreateBattleUI().Coroutine();
        await Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);

        // 加载目标关卡
        PlayerAttributeCom attribute = PlayerComponent.Instance.MyPlayer.GetComponent<PlayerAttributeCom>();
        int level = attribute.PlayerCurLevel;
        LoadMap(level).Coroutine();
    }

    async ECSVoid CreateBattleUI()
    {
        FUIComponent fuicom = Game.Scene.GetComponent<FUIComponent>();
        bool isCreated = fuicom.Check(FUIType.BattlePanel);
        FUI fui = null;
        if (isCreated == false)
        {
            fui = await BattleFactory.Create();
            fuicom.Add(fui);
        }
        else
        {
            fui = fuicom.Get(FUIType.BattlePanel);
        }

         fui.GetComponent<UIBattleComponent>().ShowBattle();
    }

    // 初始化地图

    public async ECSVoid LoadMap(int targetLevel)
    {
        //  主要是等所有的tile加载完毕
        await MapFactory.Create(targetLevel);
        // 通知可以
        Game.EventSystem.Run(EventIdType.InitBattleOver);
    }
}

[Event(EventIdType.InitBattleOver)]
public class InitBattleOver: AEvent
{
    public override void Run()
    {
        // 收到这个消息以后， UI 可以开始倒计时，GamePlay 组件可以开始执行
        RunUI();
        RunBattleCom().Coroutine();
    }

    public void RunUI()
    {
        InGameDataCom gameData = MapComponent.Inst.CurMap.GetComponent<InGameDataCom>();
        FUIComponent fuiCom = Game.Scene.GetComponent<FUIComponent>();
        FUI uiBattle = fuiCom.Get(FUIType.BattlePanel);
        uiBattle.GetComponent<UIBattleComponent>().StartBattle();
        uiBattle.GetComponent<UIBattleComponent>().UpdateLevel(gameData.CurWave, gameData.MaxWave);

        gameData.StartBallScale = BallComponent.Instance.CurBall.LocalScale;
    }

    async  ECSVoid RunBattleCom()
    {
        await Game.Scene.GetComponent<TimerComponent>().WaitAsync(500);
        BallComponent.Instance.CurBall.GetComponent<BallPostionCom>().IsSyncRacketPostion = false;
        BallSplitCom ballSplit = MapComponent.Inst.CurMap.GetComponent<BallSplitCom>();
        List<Ball> allBalls = ballSplit.GetAllBalls();
        foreach (Ball ball in allBalls)
        {
            BallMoveCom moveCom = ball.GetComponent<BallMoveCom>();
            moveCom.UpdateAngle();
        }

        RacketShootingCom shootingCom = Game.Scene.GetComponent<RacketComponent>().CurRacket.GetComponent<RacketShootingCom>();
        shootingCom.CanShooting = true;
    }

}
