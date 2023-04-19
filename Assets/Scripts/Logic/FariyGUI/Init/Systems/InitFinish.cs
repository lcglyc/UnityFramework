using System.Collections.Generic;
using ECSModel;
using MonogolyConfig;
using UnityEditor;

[Event(EventIdType.InitGameFinish)]
public class InitFinish : AEvent
{
    public override void Run()
    {
        ShowGame().Coroutine();
    }

    async ECSVoid ShowGame()
    {
        //切换到游戏场景
        await ChangeScene();
        
        InitFactory.Remove();
    }

    async ECSTask ChangeScene()
    {
        
        using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
        {
            await sceneChangeComponent.ChangeSceneAsync(SceneType.Login);
        }

        ShowMainGame();
        
        BallComponent.Instance.CurBall.Visable = true;
        RacketComponent.Instance.CurRacket.Visible = true;
    }

    void ShowMainGame()
    {
        FUIComponent fui = Game.Scene.GetComponent<FUIComponent>();
        FUI mainFui = fui.Get(FUIType.MainGamePanel);
        mainFui.Visible = true;
    }

  

   


}
