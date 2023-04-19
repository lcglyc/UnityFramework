using Cysharp.Threading.Tasks;
using ECSModel;
using FairyGUI;
public static class DebugFactory{
    
    public static async UniTask<FUI> Create() {
//  这里要自己填写PackageName
        string PackageName = "GM";
        string PanelName ="GMMainPanel";

        await UniTask.CompletedTask;
        ECSModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(PackageName);
        FUI fui = ComponentFactory.Create<FUI, GObject>(UIPackage.CreateObject(PackageName,PanelName));
        fui.Name = PanelName;
        fui.AddComponent<DebugComponent>();
        fui.Visible = false;
        return fui;
    }
}


public static class DebugBattleFactory
{
    public static async UniTask<FUI> Create()
    {
        string PanelName ="GMBattlePanel";

        await UniTask.CompletedTask;
        FUI fui = ComponentFactory.Create<FUI, GObject>(UIPackage.CreateObject("GM",PanelName));
        fui.Name = PanelName;
        fui.AddComponent<DebugBattleComponent>();
        fui.Visible = false;
        return fui;
    }
}
