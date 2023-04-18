namespace ET
{
    public abstract class AUIEvent
    {
        public abstract UniTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer);
        public abstract void OnRemove(UIComponent uiComponent);
    }
}