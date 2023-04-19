using ECSModel;
using Kunpo;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Event(EventIdType.BattleResult)]
public class BattleResultSystem : AEvent<GameState>
{
    public override void Run(GameState result)
    {
        FUIComponent fui = Game.Scene.GetComponent<FUIComponent>();
        InGameDataCom inGame = MapComponent.Inst.CurMap.GetComponent<InGameDataCom>();
        GameCtrlComponent.Instance.CurGameState = result;

        Player curPlayer = PlayerComponent.Instance.MyPlayer;
        PlayerAttributeCom atre = curPlayer.GetComponent<PlayerAttributeCom>();

        // 停止战斗主界面的动效
        StopBattlePanel();

        if (result == GameState.InGame_Defeat && !inGame.IsReplay)
        {
            inGame.IsSuccess = false;
            ProceFailPanel(inGame);
            ShowFailPanel(fui, inGame, atre.Money, atre.Diamond);
            return;
        }

        if (inGame.IsReplay)
            ProceFailPanel(inGame);

        ShowResultPanel(fui, inGame, atre.Money, atre.Diamond);
    }

    public async UniTaskVoid ShowFailPanel(FUIComponent fuiCom, InGameDataCom inGame, BigNumber money, int diamond)
    {
        FUI fui = null;
        if (fuiCom.Check(FUIType.UI_BattleFailPanel))
        {
            fui = fuiCom.Get(FUIType.UI_BattleFailPanel);
        }
        else
        {
            fui = await BattleResultFactory.CreateFailPanel();
            fuiCom.Add(fui);
        }
        BattleFailCompoent battle = fui.GetComponent<BattleFailCompoent>();

        if (fui.Visible == false)
        {
            int remaWave = inGame.MaxWave - inGame.CurWave;
            battle.Init(money, diamond, remaWave);
            fui.Visible = true;
        }
    }

    //  这里要把球都回到原点
    public void ProceFailPanel(InGameDataCom inGameDataCom)
    {
        BallSplitCom splitCom = MapComponent.Inst.CurMap.GetComponent<BallSplitCom>();
        splitCom.ReduceAllSplitBall();

        Ball ball = BallComponent.Instance.CurBall;
        ball.RemoveComponent<BallMoveCom>();
        Vector3 racket = RacketComponent.Instance.CurRacket.StartPosition;
        ball.GetComponent<BallPostionCom>().SetBallStartPostion(inGameDataCom.InGameBallScale.x);
        ball.LocalScale = inGameDataCom.InGameBallScale;
        ball.Visable = true;
    }

    public async UniTaskVoid ShowResultPanel(FUIComponent fuiCom, InGameDataCom inGame, BigNumber money, int Diamond)
    {
        FUI fui = null;
        if (fuiCom.Check(FUIType.UI_BattleResultPanel))
        {
            fui = fuiCom.Get(FUIType.UI_BattleResultPanel);
        }
        else
        {
            fui = await BattleResultFactory.CreateResultPanel();
            fuiCom.Add(fui);
        }

        BattleResultComponent battle = fui.GetComponent<BattleResultComponent>();
        if (fui.Visible == false)
        {
            fui.Visible = true;
            battle.ShowPanel(inGame.IsSuccess);
            battle.SetLevel(inGame.CurLevelID);
            battle.SetReward(inGame.InGameMoney);
            battle.SetPlayerDiamond(Diamond);
            battle.SetPlayerMoney(money);
        }
    }

    public void StopBattlePanel()
    {
        FUIComponent fuiCom = Game.Scene.GetComponent<FUIComponent>();
        FUI fui = fuiCom.Get(FUIType.BattlePanel);
        fui.GetComponent<UIBattleComponent>().EndBattle();
        fui.Visible = false;
    }
}
