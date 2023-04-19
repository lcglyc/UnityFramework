using System.Collections.Generic;
using  ECSModel;
using FairyGUI;

[ObjectSystem]
public class DebugBattleComponentSystem : AwakeSystem<DebugBattleComponent>
{
    public override void Awake(DebugBattleComponent self){
        self.Awake(self);
    }
}

public class DebugBattleComponent : Component
{
    private FUI DebugBattlePanel;
    private GButton btn_size_init, btn_time_max, btn_all_die, btn_close;
    private GSlider slider_size;
    private UIBattleComponent battleComponent;
    private float curTimeScale = 0;
    public void Awake(DebugBattleComponent self)
    {
        DebugBattlePanel = self.GetParent<FUI>();
        btn_close= FGUIHelper.GetButton("btn_close", DebugBattlePanel,OnClease);
        btn_size_init = FGUIHelper.GetButton("btn_size_init", DebugBattlePanel, OnTimeBack);
        btn_time_max = FGUIHelper.GetButton("btn_time_max", DebugBattlePanel, OnClickTime);
        btn_all_die = FGUIHelper.GetButton("btn_all_die", DebugBattlePanel, OnClickClear);
        slider_size = FGUIHelper.GetSlider("slider_size", DebugBattlePanel,OnSliderChanged);

    }
    
    public void OnInit()
    {
        DebugBattlePanel.Visible = true;
        curTimeScale = UnityEngine.Time.timeScale; 
        UnityEngine.Time.timeScale = 0;
        slider_size.max = 2.0f;
    }

    public void OnTimeBack()
    {
        curTimeScale = 1;
        UnityEngine.Time.timeScale = curTimeScale;
    }

    public void OnClickTime()
    {
        FUI uibattle = Game.Scene.GetComponent<FUIComponent>().Get(FUIType.BattlePanel);
        battleComponent  = uibattle.GetComponent<UIBattleComponent>();
        
        if (battleComponent != null)
        {
            battleComponent.DebugOrderSetTime(999);
        }
    }

    public void OnClickClear()
    {
        TipsComponent.Instance.ShowTips("暂时关闭");
        return;
        
        InGameDataCom inGame = MapComponent.Inst.CurMap.GetComponent<InGameDataCom>();
        List<int> tileids= inGame.Debug_GetThisWaveList(inGame.CurWave);
        
        for (int i = 0; i < tileids.Count -1 ; i++)
        {
            int id = tileids[i];
            Game.EventSystem.Run<int,int,int>(EventIdType.OnNotifyReduceTile,inGame.CurWave,id,100);
            Define.IsDebug = true;
        }
    }

    public void OnSliderChanged( EventContext context )
    {
        TipsComponent.Instance.ShowTips("暂时关闭");
        return;

        
        GSlider slider = (GSlider) context.sender;
        curTimeScale = (float)slider.value;
        Log.Debug(slider.value.ToString());
        slider.text = curTimeScale.ToString();
    }

    public void OnClease()
    {
        DebugBattlePanel.Visible = false;
        UnityEngine.Time.timeScale = curTimeScale;
    }
}
