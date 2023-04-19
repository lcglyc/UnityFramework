using ECSModel;
using DG.Tweening;
using UnityEngine;
using Component = ECSModel.Component;

public class BallPostionCom : Component
{
    //根据球的缩放来确定球的位置

    GameObject thisBallGo;
    Ball thisBall;
    private Rigidbody2D rigidbody2D;

    public bool IsSyncRacketPostion
    {
        get;
        set;
    }

    public void Init( float defalutScale)
    {
        thisBall = this.GetParent<Ball>();
        thisBallGo = thisBall.GameObject;
        rigidbody2D = thisBallGo.GetComponent<Rigidbody2D>();
        SetBallStartPostion(defalutScale);
        this.IsSyncRacketPostion = true;
    }


    private Vector3 BallStartPositon;
    float ballHight = 0;
    public Vector3 GetPosition( float sizeHight, float formbtom)
    {
        // Top          134/2
        //  Center      0
        // Bottom   -3.75
        // 
        float yBotton = -6.8f + formbtom + sizeHight * 0.5f;
        ballHight = sizeHight;
        BallStartPositon = new Vector3(0, yBotton, 0);
        return BallStartPositon;
    }

    public void SetBallStartPostion( float defaultScale )
    {
        float ballDia = 3.0f;// 默认
        float realdia = ballDia * defaultScale;
        Vector3 ballPos = GetPosition(realdia, 1.5f);
        thisBall.Position = ballPos;
        thisBallGo.transform.localScale = defaultScale* Vector3.one;
    }

    public void SetBallPostion( Vector3 position )
    {
        float y = position.y;
        y += ballHight * 0.5f  + 0.2f; // 0.2 是 挡板厚度
        position.y = y;

        thisBall.Position = position;
    }

    public void BallSyncRacketPostion(Vector3 position)
    {
        float y = BallStartPositon.y;
        position.y = y;
        rigidbody2D.MovePosition(position);
    }
    
    // 组合函数1， 移动到中心
    public void MoveToCenter( int index )
    {
        Vector3 position = new UnityEngine.Vector3(index * Define.BallDistance, 2.48f, 0);
        var CurSelectBallIndex = BallComponent.Instance.GetCurBallIndex();
        if (index != CurSelectBallIndex)
            thisBall.LocalPosition = position;
        else
        {
            thisBallGo.transform.DOLocalMove(position, 0.5f);
        }
    }

    //组合函数2，移动到低
    public void MoveToBottom()
    {
        thisBallGo.transform.DOMove(BallStartPositon, 0.5f);
    }

    public void MoveToLeft()
    {
        Vector3 leftPosition = new Vector3(  BallStartPositon.x-5.0f,thisBallGo.transform.localPosition.y,0);
        thisBallGo.transform.DOLocalMove(leftPosition, 0.5f);
    }
    
}
