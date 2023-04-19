
using Cysharp.Threading.Tasks;
using ECSModel;
using FairyGUI;
public static class TipsFactory
{
    public static async UniTask<FUI> Create()
    {
        await UniTask.CompletedTask;
        // 可以同步或者异步加载,异步加载需要搞个转圈圈,这里为了简单使用同步加载
        // await ECSModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackageAsync(自己写);

        if (ECSModel.Game.Scene.GetComponent<FUIPackageComponent>().HasPackage(FUIType.TipsPackage) == false)
        {
            ECSModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIType.TipsPackage);
        }

        FUI fui = ComponentFactory.Create<FUI, GObject>(UIPackage.CreateObject(FUIType.TipsPackage, FUIType.TipsPanel));
        fui.Name = FUIType.TipsPanel;

        // 挂上窗口组件就成了窗口
        FUIWindowComponent fWindow = fui.AddComponent<FUIWindowComponent>();
        fWindow.Modal = false;
        fui.AddComponent<TipsComponent>();
        fWindow.Hide();

        return fui;
    }
}