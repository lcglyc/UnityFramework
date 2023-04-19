using ECSModel;
using UnityEngine;
using Cysharp.Threading.Tasks;

[Event(EventIdType.InitStarGameFUI)]
public class InitSystem : AEvent
{
    public override void Run()
    {
        Debug.LogError("执行了  创建UI");
        // 创建CheckUI
        CreateUI();
        CreateTips();
    }

    public async UniTaskVoid CreateUI()
    {
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        // 使用工厂创建一个Loading UI
        FUI ui = await InitFactory.Create();
        fuiComponent.Add(ui);
    }

    public async UniTaskVoid CreateTips()
    {
        await Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
        await TipsFactory.Create();
    }
}
