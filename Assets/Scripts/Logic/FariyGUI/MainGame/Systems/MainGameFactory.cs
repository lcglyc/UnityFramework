
using Cysharp.Threading.Tasks;
using ECSModel;
using FairyGUI;
public static class MainGameFactory
{
    public static async UniTask<FUI> Create()
    {
        await UniTask.CompletedTask;
        // 可以同步或者异步加载,异步加载需要搞个转圈圈,这里为了简单使用同步加载
        // await ECSModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackageAsync(自己写);

        Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIType.MainGamePackage);
        FUI fui = ComponentFactory.Create<FUI, GObject>(UIPackage.CreateObject(FUIType.MainGamePackage, FUIType.MainGamePanel));
        fui.Name = FUIType.MainGamePanel;
        fui.AddComponent<MainGameComponent>();
        fui.AddComponent<UpgradeComponent>();
        fui.AddComponent<RacketUpgradeComponent>();
        return fui;
    }
}