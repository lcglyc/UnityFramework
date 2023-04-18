using Cysharp.Threading.Tasks;

namespace ET
{
    public static class UIHelper
    {
        public static async UniTask<UI> Create(Scene scene, string uiType, UILayer uiLayer)
        {
            UI ui = await scene.GetComponent<UIComponent>().Create(uiType, uiLayer);
            return ui;
        }

        public static async UniTask Remove(Scene scene, string uiType)
        {
            scene.GetComponent<UIComponent>().Remove(uiType);
            await UniTask.CompletedTask;
        }
    }
}