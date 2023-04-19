using ECSModel;
using Kunpo;

[Event(EventIdType.UpdateMainGamePanelVisable)]
public class UpdateMainGamePanelSystem : AEvent<bool>
{
    public override void Run(bool isVisable)
    {
        FUIComponent fui = Game.Scene.GetComponent<FUIComponent>();
        FUI tarUI = fui.Get(FUIType.MainGamePanel);
        tarUI.GetComponent<MainGameComponent>().UpdateGameMainPanelVisable(isVisable);
    }
}


[Event(EventIdType.UI_UpdatePlayerMoney)]
public class UpdateUIPlayerMoney : AEvent
{
    public override void Run()
    {
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        FUI mainPanel = fuiComponent.Get(FUIType.MainGamePanel);
        // 这里直接从player 属性里那数据，UI本身不管
        mainPanel.GetComponent<MainGameComponent>().UpdateMoneyUIByAttribute();
    }
}

[Event(EventIdType.UI_UpdatePlayerDiamond)]
public class UpdateUIPlayerDiamond : AEvent
{
    public override void Run()
    {
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        FUI mainPanel = fuiComponent.Get(FUIType.MainGamePanel);
        // 这里直接从player 属性里那数据，UI本身不管
        mainPanel.GetComponent<MainGameComponent>().UpdateDiamondUIByAttirbute();
    }
}

[Event(EventIdType.UI_UpdatePlayerLevel)]
public class UpdateUIPlayerLevel : AEvent
{
    public override void Run()
    {
        FUIComponent fuiComponent = Game.Scene.GetComponent<FUIComponent>();
        FUI mainPanel = fuiComponent.Get(FUIType.MainGamePanel);
        // 这里直接从player 属性里那数据，UI本身不管
        mainPanel.GetComponent<MainGameComponent>().UpdateLevelUIByAttribute();
    }
}

