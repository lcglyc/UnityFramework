using ECSModel;
using UnityEngine;

[Event(EventIdType.InitStarGameFUI)]
public class InitSystem : AEvent
{
    public override void Run()
    {
        Debug.LogError("执行了  创建UI");
        // 创建CheckUI
        CreateUI().Coroutine();
        CreateTips().Coroutine();
    }

    public async ECSVoid CreateUI()
    {
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        // 使用工厂创建一个Loading UI
        FUI ui = await InitFactory.Create();
        fuiComponent.Add(ui);
    }

    public async ECSVoid CreateTips()
    {
        await Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
        await TipsFactory.Create();
    }
}
