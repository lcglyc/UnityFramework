using FairyGUI;
using ECSModel;

[ObjectSystem]
public class TipsComponentSystem : AwakeSystem<TipsComponent>
{
    public override void Awake(TipsComponent self)
    {
        self.Awake(self);
    }
}
public class TipsComponent : Component
{
    public static TipsComponent Instance = null;
    FUIWindowComponent window;

    GTextField mTips;
    TimerComponent timer;


    public void Awake(TipsComponent self)
    {
        Instance = this;
        window = this.GetParent<FUI>().GetComponent<FUIWindowComponent>();
        FUI tips = self.GetParent<FUI>();
        window.Center();
        self.mTips = tips.Get("text").GObject.asTextField;
        timer = Game.Scene.GetComponent<TimerComponent>();
    }

    public void ShowTips(string tips)
    {
        if (window.IsShowing)
            return;

        mTips.text = tips;
        window.Show();
        Delay().Coroutine();
    }



    async ECSVoid Delay()
    {
        await timer.WaitAsync(1500);
        window.Hide();
    }
    

    
    
}