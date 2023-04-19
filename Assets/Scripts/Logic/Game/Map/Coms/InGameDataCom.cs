using ECSModel;
using Kunpo;
using System.Collections.Generic;
using UnityEngine;
using Component = ECSModel.Component;
using Log = ECSModel.Log;

// 这里用来记录游戏内产生的数据，比如钱，buffer 等

public class InGameDataCom : Component
{
    public int CurWave { get; set; }
    public int MaxWave { get; set; }

    public BigNumber InGameMoney { get; set; }

    public int CurLevelID { get; set; }

    public bool IsReplay { get; set; }

    public bool IsSuccess { get; set; }

    public  int BossWave { get; set; }

    public Vector3 StartBallScale { get; set; }

    public Vector3 InGameBallScale
    { 
        get;
        set;
    }

    private Dictionary<int, List<int>> mThisLevelTiles;

    public void Init(int levelID)
    {
        JsonLibComponent jsonlib = Game.Scene.GetComponent<JsonLibComponent>();
        
        CurWave = 1;
        CurLevelID = levelID;
        IsReplay = false;
        IsSuccess = false;
        InGameMoney = new BigNumber();
        InGameMoney.SetToZero();
        this.BossWave = jsonlib.GetLevelBossWave(levelID);
        mThisLevelTiles = new Dictionary<int, List<int>>();
    }

    public void AddMoney(int singleMoney)
    {
        InGameMoney += singleMoney;
        Game.EventSystem.Run<BigNumber>(EventIdType.UI_UpdateBattleMoney, InGameMoney);
    }

    public void UpdateReplayState( bool isused)
    {
        this.IsReplay = isused;
    }


    public void OnAddTileData( int wave,int id )
    {
        if (!mThisLevelTiles.ContainsKey(wave))
        {
            mThisLevelTiles.Add(wave, new List<int>());
        }
        
        mThisLevelTiles[wave].Add(id);
    }

    public int OnReduceTile( int wave, int id )
    {
        if (mThisLevelTiles.ContainsKey(wave))
        {
            mThisLevelTiles[wave].Remove(id);
        }

        int count = mThisLevelTiles[wave].Count;
        return count;
    }

    public int GetRestTiles()
    {
        int number = 0;
        foreach (var value in mThisLevelTiles.Values)
        {
            number += value.Count;
        }

        return number;
    }

    public int  GetThisWaveTiles( int wave )
    {
        int tiles = -1;
        if (mThisLevelTiles.ContainsKey(wave))
            tiles = mThisLevelTiles[wave].Count;

        return tiles;
    }

    public List<int> Debug_GetThisWaveList( int id)
    {
        return mThisLevelTiles[id];
    }
    
    
    public override void Dispose()
    {
        this.CurWave = 0;
        this.MaxWave = 0;
        this.BossWave = -1;
        this.IsReplay = false;
        this.IsSuccess = false;
        this.CurLevelID = 0;
        mThisLevelTiles.Clear();
        
        this.StartBallScale = Vector3.zero;
        this.InGameBallScale= Vector3.zero;
        
        base.Dispose();
    }
}
