using ECSModel;
using System.Collections.Generic;
using UnityEngine;

[Event(EventIdType.InitRacketUpgradePanel)]
public class InitRacketUpgradePanel : AEvent
{
    public override void Run()
    {
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        FUI mainPanel = fuiComponent.Get(FUIType.MainGamePanel);
        var RacketUpgradeCom = mainPanel.GetComponent<RacketUpgradeComponent>();
        RacketUpgradeCom.Init();
    }
}

[Event(EventIdType.CloseRacketUpgradePanel)]
public class CloseRacketUpgradePanel : AEvent
{
    public override void Run()
    {
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        FUI mainPanel = fuiComponent.Get(FUIType.MainGamePanel);
        var RacketUpgradeCom = mainPanel.GetComponent<RacketUpgradeComponent>();
        RacketUpgradeCom.OnClosePanel();
        MoveBackRacket();
        CheckRakcet();
    }

    public void MoveBackRacket()
    {
        Racket racket = RacketComponent.Instance.CurRacket;
        racket.GetComponent<RacketPosCom>().MoveToBottom();
    }
    
    void CheckRakcet()
    {
        long id = BallComponent.Instance.CurBall.Id;
        Game.EventSystem.Run<long,bool>( EventIdType.UpdateBallAlpha,id,true);

        Vector3 racket = RacketComponent.Instance.CurRacket.StartPosition;
        BallComponent.Instance.CurBall.GetComponent<BallPostionCom>().SetBallPostion(racket);
    }
    
}


