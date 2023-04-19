using ECSModel;
using System.Collections.Generic;

/// <summary>
///  这里记录了玩家解锁的挡板，以及当前正在使用的挡板
/// </summary>
public class PlayerBarCom : Component
{
    List<int> mPlayerBarCom;
    int barID = 0;

    // todo：要处理获取同一个球的逻辑
    public void AddBalls(int id)
    {
        if (mPlayerBarCom == null)
            mPlayerBarCom = new List<int>();

        mPlayerBarCom.Add(id);
    }

    List<int> GetPlayerBalls()
    {
        return mPlayerBarCom;
    }

    // 设置当前球
    public int CurBarID
    {
        get => barID;
        set => barID = value;
    }
}
