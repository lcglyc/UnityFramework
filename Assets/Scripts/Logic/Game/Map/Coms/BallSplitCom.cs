using ECSModel;
using System.Collections.Generic;
using UnityEngine;
using Component = ECSModel.Component;

// 一个主球，分裂成多个小球,这里直接克隆 curball
public class BallSplitCom : Component
{
    List<Ball> InGameBalls = null;
    long mainBallID;
   public long MainBallID {
        get => mainBallID;
    }
    public async ECSVoid Init(int number,int configid,float speed,Ball mainBall)
    {
        InGameBalls = new List<Ball>();
        // 因为没有主球的概念，所以主副球都在一个list里面
        InGameBalls.Add(mainBall);
        mainBallID = mainBall.Id;
        BallAttributeCom mainBallAttribute = mainBall.GetComponent<BallAttributeCom>();
        // 给所有的球添加移动属性
        for (int i = 1; i <number; i++)
        {
            long id = RandomHelper.RandInt64();
            Ball tmpBall =await BallFactory.Create(id, configid,mainBallAttribute);
            tmpBall.GameObject.transform.parent = BallComponent.Instance.GameObject.transform;
            BallMoveCom move = tmpBall.AddComponent<BallMoveCom>();
            tmpBall.LocalScale = mainBall.LocalScale;
            float x = 0.1f;
            if ( i %2 ==0)
            {
                x = (i - 1) * x;
            }
            else
            {
                x = -1.0f * i * x;
            }

            int delayTime = i * 10;
            move.Init(speed, x,delayTime);
            InGameBalls.Add(tmpBall);
        }
    }

    public void ReduceAllSplitBall()
    {
        foreach (Ball ball in InGameBalls)
        {
            if (ball.Id == mainBallID)
                ball.Visable = false;
            else
                ball.Dispose();
        }

        InGameBalls.Clear();
    }

    public bool  ReduceBall( long id)
    {
        // 如果就剩下1个球，就不要减了，启动重置
        if (InGameBalls.Count == 1)
            return false;

        Ball ball = GetBall(id);
        if (ball == null) return false;

        // 如果这个球是 curball ，就不删除，只是隐藏即可

        if (ball.Id == mainBallID)
            ball.Visable = false;
        else
            ball.Dispose();

        InGameBalls.Remove(ball);
        return true;
    }


    public Ball GetBall(long id)
    {
        return InGameBalls.Find(x => x.Id == id);
    }

    public List<Ball> GetAllBalls()
    {
        return InGameBalls;
    }

    public override void Dispose()
    {
        if (InGameBalls != null)
        {
            InGameBalls.Clear();
        }
        
        base.Dispose();
    }
}
