using ECSModel;
using UnityEngine;
using FairyGUI;
using Component = ECSModel.Component;
using Debug = System.Diagnostics.Debug;
using IFilter = FairyGUI.IFilter;

[ObjectSystem]
 class RacketUpdateSystem:UpdateSystem<RacketMoveCom>
{
    public override void Update(RacketMoveCom self)
    {
        self.Update();
    }
}

[ObjectSystem]
 class RacketComAwakeSystem : AwakeSystem<RacketMoveCom>
{
    public override void Awake(RacketMoveCom self)
    {
        self.Awake();
    }
}

public class RacketMoveCom :Component
{
    Vector3 StartPosition = Vector3.zero;
    GameObject racketObject;
    public float speed = 10.0f;
    private Rigidbody2D moveBody;
    private BallPostionCom BallSyncPostion;
    public void Awake()
    {
        StartPosition = new Vector3(0, -5.5f, 0);
        racketObject = this.GetParent<Racket>().GameObject;
        racketObject.transform.position = StartPosition;
        moveBody = racketObject.GetComponent<Rigidbody2D>();
        BallSyncPostion = BallComponent.Instance.CurBall.GetComponent<BallPostionCom>();
    }

    public void MoveToStartPostion()
    {
        racketObject.transform.position = StartPosition;
    }

    public void Update()
    {
        var mainPanelCom = Game.Scene.GetComponent<FUIComponent>().Get(FUIType.MainGamePanel).GetComponent<MainGameComponent>();
        if(mainPanelCom.isChallengePanel) EditorDrag(); 
    }

    private Vector3 m_screenPos = Vector3.zero;
    private bool IsMoving = false;
    
    void EditorDrag()
    {
        if( Input.GetMouseButtonDown(0) && !Stage.isTouchOnUI)
        {
            IsMoving = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            IsMoving = false;
        }
        
        if (IsMoving )
        {
            Vector3 offset =Camera.main.ScreenToWorldPoint( Input.mousePosition )- m_screenPos;
            float x = offset.x;
            offset.x = Mathf.Clamp(x, -2.8f, 2.8f);
            offset.y = -5.5f;
            Vector3 position = offset + racketObject.transform.right * Time.deltaTime;
            moveBody.MovePosition(position);
            if (BallSyncPostion.IsSyncRacketPostion ) BallSyncPostion.BallSyncRacketPostion(position);    
        }
    }
    
    

}
