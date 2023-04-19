using Cysharp.Threading.Tasks;
using FairyGUI;
using ECSModel;
using Kunpo;

[ObjectSystem]
public class BattleFailCompoentSystem : AwakeSystem<BattleFailCompoent>
{
    public override void Awake(BattleFailCompoent self)
    {
        self.Awake(self);
    }
}
public class BattleFailCompoent : Component
{
    FUI resultPanel;
    GButton needCrysBtn;
    GTextField gold_num, crys_num, text_tips_1, text_tips_2, text_num;

    string tips1, tips2;
    int needCrys;
    int reduceTime = 5;
    JsonLibComponent jsonlib;
    TimerComponent timer;
    bool OnClickReplay = false;

    public void Awake(BattleFailCompoent self)
    {
        resultPanel = self.GetParent<FUI>();
        resultPanel.GObject.asCom.MakeFullScreen();
        text_tips_1 = resultPanel.Get("text_tips_1").GObject.asTextField;
        text_tips_2 = resultPanel.Get("text_tips_2").GObject.asTextField;
        text_num = resultPanel.Get("text_num").GObject.asTextField;

        gold_num = resultPanel.Get("gold_num").GObject.asTextField;
        crys_num = resultPanel.Get("crys_num").GObject.asTextField;
        needCrysBtn = FGUIHelper.GetButton("btn_cost", resultPanel, OnButtonClick);

        tips1 = text_tips_1.text;
        tips2 = text_tips_2.text;
        OnClickReplay = false;
        jsonlib = Game.Scene.GetComponent<JsonLibComponent>();
        timer = Game.Scene.GetComponent<TimerComponent>();
    }

    int curDiamod;
    public void Init(BigNumber number, int diamond, int remaWave)
    {
        int tmpWave = remaWave > 0 ? remaWave : 1;
        gold_num.text = number.ToStringD3();
        crys_num.text = diamond.ToString();
        text_tips_1.text = string.Format(tips1, tmpWave);

        int time = jsonlib.GetInGameReviveTime();
        needCrys = jsonlib.GetInGameReviveNeedCrys();
        text_tips_2.text = string.Format(tips2, time);
        curDiamod = diamond;
        needCrysBtn.text = needCrys.ToString();
        reduceTime = 5;
        OnClickReplay = false;
        ReduceFiveTime();
    }

    void OnButtonClick()
    {
        //if (curDiamod <needCrys)
        //{
        //    TipsComponent.Instance.ShowTips("Text_Global_Tips_CrysNotEnough");
        //    return;
        //}
        OnClickReplay = true;
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        fuiComponent.Remove(FUIType.UI_BattleFailPanel);
        fuiComponent.Get(FUIType.BattlePanel).Visible = true;
        Game.EventSystem.Run(EventIdType.UsedDiamondRePlayer);
    }

    void OutTime()
    {
        Game.Scene.GetComponent<FUIComponent>().Remove(FUIType.UI_BattleFailPanel);
        Game.EventSystem.Run<GameState>(EventIdType.BattleResult, GameState.InGame_Reslut);
    }


    void ReduceFiveTime()
    {
        UpdateReduce();
    }

    async UniTaskVoid UpdateReduce()
    {
        if (OnClickReplay)
        {
            reduceTime = 5;
            return;
        }


        reduceTime--;
        if (reduceTime < 0)
        {
            return;
        }
        text_num.text = reduceTime.ToString();

        if (reduceTime == 0)
        {
            OutTime();
            return;
        }

        await timer.WaitAsync(1000);
        UpdateReduce();
    }

    public override void Dispose()
    {
        OnClickReplay = false;
        reduceTime = 0;
        base.Dispose();
    }
}