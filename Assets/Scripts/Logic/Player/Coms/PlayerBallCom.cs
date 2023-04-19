using ECSModel;
using System.Collections.Generic;

/// <summary>
///  这里记录了玩家解锁的球(初始状态的球)，以及当前正在使用的球
/// </summary>
public class PlayerBallCom : Component
{

    List<int> mPlayerBalls;
    int curBall = 0;

    // todo：要处理获取同一个球的逻辑
    public void AddBalls(int id)
    {
        if (mPlayerBalls ==null)
            mPlayerBalls = new List<int>();

        mPlayerBalls.Add(id);
    }

    List<int> GetPlayerBalls()
    {
        return mPlayerBalls;
    }

    // 设置当前球
    public int CurBallID
    {
        get => curBall;
        set => curBall = value;
    }
}
