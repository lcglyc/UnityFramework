
using ECSModel;
using System.Collections.Generic;

[Event(EventIdType.UsedDiamondRePlayer)]
public class OnReplaySystem : AEvent
{
    //  重置时间
    // 
    public override void Run()
    {
        ReloadBall();
        ReloadUI();
    }


    private void ReloadBall()
    {
        MapFactory.AddBallCom(MapComponent.Inst.CurMap);
        BallSplitCom ballSplit = MapComponent.Inst.CurMap.GetComponent<BallSplitCom>();
        List<Ball> allBalls = ballSplit.GetAllBalls();
        long mainBallID = ballSplit.MainBallID;
        foreach (Ball ball in allBalls)
        {
            if (ball.Id == mainBallID)
                ball.Visable = true;

            BallMoveCom moveCom = ball.GetComponent<BallMoveCom>();
            moveCom.UpdateAngle();
        }
    }

    private void ReloadUI()
    {
        GameCtrlComponent.Instance.CurGameState = GameState.INGAMEMAP;
        FUIComponent fuiCom = Game.Scene.GetComponent<FUIComponent>();
        FUI ui = fuiCom.Get(FUIType.BattlePanel);
        ui.GetComponent<UIBattleComponent>().ReStartBattle(8);

        InGameDataCom inGame = MapComponent.Inst.CurMap.GetComponent<InGameDataCom>();
        inGame.IsReplay = true;
    }

}
