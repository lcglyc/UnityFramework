using ECSModel;
using UnityEngine;

[Event(EventIdType.OnBallOutFrame)]
class OnBallOutFrameSystem : AEvent<long>
{
    public override void Run(long a)
    {
        if (MapComponent.Inst.CurMap == null)
            return;

        GameState mState = GameCtrlComponent.Instance.CurGameState;
        if (mState < GameState.INGAMEMAP )
        {
            return;
        }
        BallSplitCom splitcom = MapComponent.Inst.CurMap.GetComponent<BallSplitCom>();
        // 如果减少失败，表示就剩下1个球了，重置&减少时间
        if (splitcom.ReduceBall(a) == false)
        {
            // 如果是在结算界面，
            if(  mState == GameState.InGame_Defeat || mState == GameState.InGame_Reslut )
            {
                splitcom.ReduceAllSplitBall();
                ProcessResultState();
                return;
            }

            // 在游戏中，如果最后一个球丢失，就拿到最后一个球的位置，并且重置，减时间            
            if (mState == GameState.INGAMEMAP)
            {
                // 这里应该是获取到挡板的位置
                ResetPostion(a, splitcom);
                // 通知UI去减少时间，每次减少5秒
                Game.EventSystem.Run(EventIdType.UI_UpdateBattleTime);
            }
        }
    }

    void ResetPostion(  long id, BallSplitCom splitcom)
    {
        Vector3 racket = RacketComponent.Instance.CurRacket.Position;
        // 设置最后一个球的位置
        Ball lastBall = splitcom.GetBall(id);
        lastBall.GetComponent<BallPostionCom>().SetBallPostion(racket);
    }

    void ProcessResultState()
    {
        Vector3 racket = RacketComponent.Instance.CurRacket.StartPosition;
        Ball ball = BallComponent.Instance.CurBall;
        ball.Visable = true;
        ball.RemoveComponent<BallMoveCom>();
        ball.GetComponent<BallPostionCom>().SetBallPostion(racket);
    }
}


