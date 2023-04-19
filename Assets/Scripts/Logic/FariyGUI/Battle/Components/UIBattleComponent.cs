using FairyGUI;
using ECSModel;
using Kunpo;
[ObjectSystem]
public class UIBattleComponentSystem : AwakeSystem<UIBattleComponent>
{
    public override void Awake(UIBattleComponent self)
    {
        self.Awake(self);
    }
}
public class UIBattleComponent : Component
{
    FUI mBattlePanel;
    Transition GameOver, BossWarning, TimeWarning, TimeWarning2, TimeWarning3, TimeWarning4, GameStart;
    TimerComponent TimerCom;
    GTextField timer_num, timer_num_large, wave_num, money_num;
    private GButton btn_gm;
    private int TimerIndex=31;

    public void Awake(UIBattleComponent self)
    {
        mBattlePanel = self.GetParent<FUI>();
        TimerCom = Game.Scene.GetComponent<TimerComponent>();
        
        GameStart = mBattlePanel.GetTransition("GameStart");
        GameOver = mBattlePanel.GetTransition("GameOver");
        BossWarning = mBattlePanel.GetTransition("BossWarning");
        TimeWarning = mBattlePanel.GetTransition("TimeWarning");
        TimeWarning2 = mBattlePanel.GetTransition("TimeWarning2");
        TimeWarning3 = mBattlePanel.GetTransition("TimeWarning3");
        TimeWarning4 = mBattlePanel.GetTransition("TimeWarning4");
        
        timer_num = mBattlePanel.Get("timer_num").GObject.asTextField;
        money_num = mBattlePanel.Get("money_num").GObject.asTextField;
        timer_num_large = mBattlePanel.Get("timer_num_large").GObject.asTextField;
        wave_num = mBattlePanel.Get("wave_num").GObject.asTextField;
        
        btn_gm = FGUIHelper.GetButton("btn_gm", mBattlePanel, OnClickGM);
    }

    public void ShowBattle()
    {
        mBattlePanel.Visible = true;
        GameStart.Play();
        timer_num.text = "0:30";
        wave_num.text = "0/0";
        money_num.text = "0";
        TimerIndex = 31;
    }

    public void StartBattle()
    {
        StartTime().Coroutine();
    }

    public void ReStartBattle( int addTime )
    {
        TimerIndex = addTime;
        StartTime().Coroutine();
    }

    public void EndBattle()
    {
        TimerIndex = 0;
        TimeWarning.Stop();
        TimeWarning2.Stop();
        TimeWarning3.Stop();
        timer_num_large.text = TimerIndex.ToString();
    }

    async ECSVoid StartTime()
    {
        await ReduceTimer();
    }

    public void ReduceTime( int reduceNum)
    {
        DoReduceTime(reduceNum);
        
        if (!TimeWarning4.playing)
        {
            TimeWarning4.Play();
        }

        ShowTime();
        DoDefeatEvent();
    }

    private  void DoReduceTime( int index )
    {
        TimerIndex = TimerIndex - index;
        TimerIndex = TimerIndex < 0 ? 0 : TimerIndex;
    }

    private void DoDefeatEvent()
    {
        if (TimerIndex <= 0)
        {
            EndBattle();
            Game.EventSystem.Run<GameState>(EventIdType.BattleResult, GameState.InGame_Defeat);
        }
    }

    private async ECSTask ReduceTimer()
    {
        if (GameCtrlComponent.Instance.CurGameState == GameState.MAINPANEL)
            return;

        TimerIndex--;
        if (TimerIndex < 0) return;

        ShowTime();

        if ( TimerIndex <= 15)
        {
            TimeWarning.Play();
        }

        if( TimerIndex <= 9 )
        {
            TimeWarning.Stop();
            TimeWarning2.Play();
            TimeWarning3.Play();
            timer_num_large.text = TimerIndex.ToString();
        }

        if (TimerIndex == 0)
        {
            DoDefeatEvent();
            return;
        }

        await TimerCom.WaitAsync(1000);
        await ReduceTimer();
    }

    private void ShowTime()
    {
        string timeMsg = TimerIndex >= 10 ? "0:{0}" : "0:0{0}";
        timer_num.text = string.Format(timeMsg, TimerIndex.ToString());
    }

    public void HideBattle()
    {
        Game.Scene.GetComponent<FUIComponent>().Remove(FUIType.BattlePanel);
    }


    public void UpdateMoney( BigNumber money )
    {
        money_num.text = money.ToStringD3();
    }

    public void UpdateLevel( int level ,int maxLevel )
    {
        string text = string.Format("{0}/{1}", level, maxLevel);
        wave_num.text = text;
    }


    public void DebugOrderSetTime( int newTime )
    {
        TimerIndex = newTime;
    }

    public void ShowBossWarning()
    {
        if (BossWarning != null)
        {
            BossWarning.Play();    
        }
    }
    

    public override void Dispose()
    {

        GameStart = null;
        GameOver = null;
        BossWarning = null;
        TimeWarning = null;
        TimeWarning2 = null;
        TimeWarning3 = null;
        TimeWarning4 = null;
        timer_num = null;
        timer_num_large = null;
        wave_num = null;
        this.TimerCom = null;

        base.Dispose();
    }


    public void OnClickGM()
    {
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        fuiComponent.Get("GMBattlePanel").GetComponent<DebugBattleComponent>().OnInit();
    }

}