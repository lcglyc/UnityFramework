
using Cysharp.Threading.Tasks;
using ECSModel;
using FairyGUI;
public static class BattleFactory
{
    public static async UniTask<FUI> Create()
    {
        await UniTask.CompletedTask;
        FUI fui = ComponentFactory.Create<FUI, GObject>(UIPackage.CreateObject(FUIType.MainGamePackage, FUIType.BattlePanel));
        fui.Name = FUIType.BattlePanel;
        fui.AddComponent<UIBattleComponent>();
        fui.Visible = false;
        return fui;
    }
}