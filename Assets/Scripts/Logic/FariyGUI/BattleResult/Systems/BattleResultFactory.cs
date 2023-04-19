using ECSModel;
using FairyGUI;
public static class BattleResultFactory
{
    public static async ECSTask<FUI> CreateResultPanel()
    {
        await ECSTask.CompletedTask;
        FUI fui = ComponentFactory.Create<FUI, GObject>(UIPackage.CreateObject(FUIType.MainGamePackage, FUIType.UI_BattleResultPanel));
        fui.Name = FUIType.UI_BattleResultPanel;
        fui.AddComponent<BattleResultComponent>();
        fui.GObject.asCom.fairyBatching = true;
        fui.Visible = false;
        return fui;
    }

    public static async ECSTask<FUI> CreateFailPanel()
    {
        await ECSTask.CompletedTask;
        FUI fui = ComponentFactory.Create<FUI, GObject>(UIPackage.CreateObject(FUIType.MainGamePackage, FUIType.UI_BattleFailPanel));
        fui.Name = FUIType.UI_BattleFailPanel;
        BattleFailCompoent battle = fui.AddComponent<BattleFailCompoent>();
        fui.Visible = false;
        fui.GObject.asCom.fairyBatching = true;
        return fui;
    }

}