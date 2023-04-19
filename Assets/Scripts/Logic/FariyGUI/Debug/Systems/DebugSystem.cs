using ECSModel;
[Event(EventIdType.OpenDebugPanel)]
public class DebugSystem : AEvent
{
    public override void Run()
    {
        GameCtrlComponent.Instance.CurGameState = GameState.GM;
        FUIComponent component = Game.Scene.GetComponent<FUIComponent>();
        FUI fui=  component.Get(FUIType.UI_DebugPanel);
        fui.GetComponent<DebugComponent>().OnInit();
        
        if (fui.Visible ==false)
            fui.Visible = true;
    }
}

