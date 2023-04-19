
using ECSModel;
using FairyGUI;
using Cysharp.Threading.Tasks;
public static class InitFactory
{
    public static async UniTask<FUI> Create()
    {
        await UniTask.CompletedTask;

        Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIType.LoadingPacage);
        FUI fui = ComponentFactory.Create<FUI, GObject>(UIPackage.CreateObject(FUIType.LoadingPacage, FUIType.LoadingPanel));
        fui.Name = FUIType.LoadingPanel;
        fui.AddComponent<InitComponent>();
        return fui;
    }

    public static void Remove()
    {
        Game.Scene.GetComponent<FUIComponent>().Remove(FUIType.LoadingPanel);
        Game.Scene.GetComponent<FUIPackageComponent>().RemovePackage(FUIType.LoadingPacage);
    }

}