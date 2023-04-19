using ECSModel;
using System;
using System.Collections.Generic;


[ObjectSystem]
public  class SerializeationDataSystem:AwakeSystem<SerializationComponent>
{
    public override void Awake(SerializationComponent self)
    {
        self.Awake();
    }
}


public class SerializationComponent : Component
{
    public void Awake()
    {

    }

    public void ClearAll()
    {
      
    }

    #region Player 序列化/ 反序列化
    public PlayerAttributeCom GetSerializePlayerAttributeCom()
    {
        return null;
    }

    public void SerializeationPlayerAttributeCom( PlayerAttributeCom com )
    {
        
    }

    public void SerializetionAllBall()
    {
        
    }
    #endregion


    #region 球 序列化/反序列化

    
    public Dictionary<long, BallAttributeCom> GetSerializeBallAttributeCom()
    {
        return null;
    }

    #endregion


    #region 板 序列化/反序列化

    public   Dictionary<long, RacketAttributeCom>  GetSerializeRacketAttributeCom()
    {
        return null;
    }
    
    public void SerializetionAllRacket()
    {
        // Dictionary<long, RacketAttributeCom> mAllRacket = new Dictionary<long, RacketAttributeCom>();
        // Racket[] rackets = RacketComponent.Instance.GetAll();
        // foreach (var VARIABLE in rackets)
        // {
        //     RacketAttributeCom com = VARIABLE.GetComponent<RacketAttributeCom>();
        //     mAllRacket.Add(VARIABLE.Id,com);
        // }
        //
        // racketES3File.Save< Dictionary<long, RacketAttributeCom>>("RacketAttributeCom",mAllRacket);
    }
    
    #endregion

    public void ReadConfig()
    {
        // if (playerES3File.GetKeys().Length ==0)
        // {
        //     return;
        // }
        //
        // if (ballES3Feile.GetKeys().Length ==0)
        // {
        //     return;
        // }
        //
        // if (racketES3File.GetKeys().Length ==0)
        // {
        //     return;
        // }
    }
    
    private void Sync()
    {
        // playerES3File.Sync();
        // ballES3Feile.Sync();
        // racketES3File.Sync();
    }

    public override void Dispose()
    {
        Sync();
        base.Dispose();
    }
}
