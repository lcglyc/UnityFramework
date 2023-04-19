using FairyGUI;
using ECSModel;
using Kunpo;

[ObjectSystem]
public class BattleResultComponentSystem : AwakeSystem<BattleResultComponent>
{
    public override void Awake(BattleResultComponent self)
    {
        self.Awake(self);
    }
}
public class BattleResultComponent : Component
{
    Transition Vitory,Fail;
    FUI resultPanel;
    GTextField text_gold_num, gold_num, crys_num;
    GButton GetGiftBtn;
    GTextField level_0_num, level_1_num, level_2_num, level_3_num;

    BigNumber tmpReward;

    public void Awake(BattleResultComponent self)
    {
        resultPanel = self.GetParent<FUI>();
        resultPanel.GObject.asCom.MakeFullScreen();
        Vitory = resultPanel.GetTransition("Vitory");
        Fail = resultPanel.GetTransition("Fail");
        text_gold_num = resultPanel.Get("text_gold_num").GObject.asTextField;

        level_0_num = resultPanel.Get("level_0_num").GObject.asTextField;
        level_1_num = resultPanel.Get("level_1_num").GObject.asTextField;
        level_2_num = resultPanel.Get("level_2_num").GObject.asTextField;
        level_3_num = resultPanel.Get("level_3_num").GObject.asTextField;

        gold_num = resultPanel.Get("gold_num").GObject.asTextField;
        crys_num = resultPanel.Get("crys_num").GObject.asTextField;

        GetGiftBtn = FGUIHelper.GetButton("btn_reward",  resultPanel, OnClickReward);
    }

    public void SetLevel( int curLevel)
    {
        int tmpLevel = curLevel;
        level_0_num.text = (--tmpLevel).ToString();
        level_1_num.text = curLevel.ToString();
        level_2_num.text = (++curLevel).ToString();
        level_3_num.text = (++curLevel).ToString();
    }

    public void SetReward( BigNumber number )
    {
        tmpReward = number;
        text_gold_num.text = number.ToStringD3();
    }

    public void SetPlayerMoney( BigNumber number )
    {
        gold_num.text = number.ToStringD3();
    }

    public void SetPlayerDiamond( int number)
    {
        crys_num.text = number.ToString();
    }

    public void ShowPanel( bool isSuccess)
    {
        if (isSuccess)
        {
            Vitory.Play();    
        }
        else
        {
            Fail.Play();
        }
        
    }

    public void OnClickReward()
    {
        PlayerAttributeCom attribute = PlayerComponent.Instance.MyPlayer.GetComponent<PlayerAttributeCom>();
        attribute.AddMoney(tmpReward);

        Game.EventSystem.Run(EventIdType.BattleOver);
        Game.Scene.GetComponent<FUIComponent>().Remove(FUIType.UI_BattleResultPanel);
    }
}