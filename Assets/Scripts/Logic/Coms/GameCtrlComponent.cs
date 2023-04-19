using ECSModel;
using UnityEngine;
using FairyGUI;

using Component = ECSModel.Component;

[ObjectSystem]
public class GameCtrlAwakeSystem : AwakeSystem<GameCtrlComponent>
{
    public override void Awake(GameCtrlComponent self)
    {
        self.Awake();
    }
}

[ObjectSystem]
public class GameCtrlUpdateSystem:UpdateSystem<GameCtrlComponent>
{
    public override void Update(GameCtrlComponent self)
    {
        self.Update();
    }
}

public class GameCtrlComponent : Component
{
    //public enum GameState
    //{
    //    NONE=0,
    //    INGAMEMAP=1,
    //     MAINPANEL=2,
    //     UPGRADEPANEL = 3
    //}

    public static GameCtrlComponent Instance = null;
    public GameState CurGameState = GameState.NONE;
    public void Awake()
    {
        Instance = this;
        CurGameState = GameState.MAINPANEL;
    }

    Vector3 mInputPosition = Vector3.zero;
    bool mStartListen = false;
    void CheckGameState()
    {
        if (CurGameState == GameState.INGAMEMAP)
            return;

        if( CurGameState == GameState.MAINPANEL )
        {
            if( Input.GetMouseButtonDown(0)  && Stage.isTouchOnUI ==false )
            {
                mInputPosition = Input.mousePosition;
                mStartListen = true;
            }

            if( Input.GetMouseButton(0) && mStartListen)
            {
                float dis = Vector3.Distance(mInputPosition, Input.mousePosition);
                if(dis >= 20)
                {
                    CurGameState = GameState.INGAMEMAP;
                    mStartListen = false;

                    Game.EventSystem.Run(EventIdType.InitBattle);
                }
            }

            if( Input.GetMouseButtonUp(0))
            {
                mStartListen = false;
            }

        }

    }

    public void Update()
    {
        CheckGameState();
    }


}
