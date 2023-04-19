using ECSModel;
using UnityEngine;

[Event(EventIdType.BattleOver)]
public class BattleOverSystem : AEvent
{
    public override void Run()
    {
        // 将球放回到起始点,去掉移动组件
        RemoveMove();
        ChangeUI();
        ClearMap();
        StopShooting();
        GameCtrlComponent.Instance.CurGameState = GameState.MAINPANEL;
    }


    void StopShooting()
    {
        RacketShootingCom shootingCom = Game.Scene.GetComponent<RacketComponent>().CurRacket.GetComponent<RacketShootingCom>();
        shootingCom.CanShooting = false;
        shootingCom.ClearInSceneBullets();
    }

    public void RemoveMove()
    {
        Ball ball = BallComponent.Instance.CurBall;
        ball.LocalScale = MapComponent.Inst.CurMap.GetComponent<InGameDataCom>().StartBallScale;
        // 移除掉分裂的球
        RemoveSplitBalls();

        // 重置球和板的位置
        FadebackBall(ball);
    }

    public void ChangeUI()
    {
        FUIComponent fuiCom = Game.Scene.GetComponent<FUIComponent>();
        FUI battle = fuiCom.Get(FUIType.BattlePanel);
        battle.GetComponent<UIBattleComponent>().HideBattle();

        // 如果是通关状态，就把奖励给同步出去

        if ( GameCtrlComponent.Instance.CurGameState == GameState.InGame_Reslut)
        {
            PlayerAttributeCom attricom = PlayerComponent.Instance.MyPlayer.GetComponent<PlayerAttributeCom>();
            InGameDataCom inGameCom = MapComponent.Inst.CurMap.GetComponent<InGameDataCom>();
            attricom.AddMoney(inGameCom.InGameMoney);

            if(inGameCom.IsSuccess)
                attricom.PlayerCurLevel = ++inGameCom.CurLevelID;
        }

        FUI main = fuiCom.Get(FUIType.MainGamePanel);
        main.GetComponent<MainGameComponent>().UpdateGameMainPanelVisable(true);
    }

    public void RemoveSplitBalls()
    {
        BallSplitCom split = MapComponent.Inst.CurMap.GetComponent<BallSplitCom>();
        if (split == null)
            return;

        split.ReduceAllSplitBall();
    }
    
    public void FadebackBall(Ball ball)
    {
        ball.RemoveComponent<BallSplitCom>();
        ball.RemoveComponent<BallMoveCom>();

        BallPostionCom position =  ball.GetComponent<BallPostionCom>();
        position.SetBallStartPostion(ball.LocalScale.x);
        position.IsSyncRacketPostion = true;
        ball.Visable = true;
    }

    public  void ClearMap()
    {
        MapComponent.Inst.CurMap.Dispose();

        WaveEntity[] allwaves = WaveComponent.Instance.GetAll();
        foreach (WaveEntity entity in allwaves)
        {
            WaveComponent.Instance.RemoveAndDiopose(entity.Id);
        }
    }

}

