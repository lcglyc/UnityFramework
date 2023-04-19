using ECSModel;
using System.Collections.Generic;
using UnityEngine;

[Event(EventIdType.InitUpgradePanel)]
public class InitUpgradePanelSystem : AEvent
{
    public override void Run()
    {
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        FUI mainPanel = fuiComponent.Get(FUIType.MainGamePanel);
        var upgradeCom = mainPanel.GetComponent<UpgradeComponent>();
        upgradeCom.Init();
    }
}

[Event(EventIdType.CloseUpgradePanel)]
public class CloseUpgradePanel : AEvent<GameState>
{
    public override void Run( GameState nextState )
    {
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        FUI mainPanel = fuiComponent.Get(FUIType.MainGamePanel);
        var upgradeCom = mainPanel.GetComponent<UpgradeComponent>();
        upgradeCom.OnClosePanel();
        if (nextState == GameState.MAINPANEL || nextState == GameState.MAINPSTORE)
        {
            MoveBackBall();
            CheckRakcet();

        }else if (nextState == GameState.MAINCAR)
        {
            MoveToLeft();
        }
    }

    void MoveBackBall()
    {
        Ball ball =  BallComponent.Instance.CurBall;
        float scale = ball.GetComponent<BallAttributeCom>().BallConfigData.DefalutScale;
        ball.GetComponent<BallPostionCom>().MoveToBottom();
    }

    void CheckRakcet()
    {
        long id = RacketComponent.Instance.CurRacket.Id;
        Game.EventSystem.Run<long,bool>( EventIdType.UpdateRacketAlpha,id,true);
        RacketComponent.Instance.CurRacket.GetComponent<RacketPosCom>().SetRacketStartPostion();
    }

    void MoveToLeft()
    {
        Ball ball =  BallComponent.Instance.CurBall;
        ball.GetComponent<BallPostionCom>().MoveToLeft();
    }

}


