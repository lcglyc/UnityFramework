using System.Collections.Generic;
using ECSModel;
using MonogolyConfig;
using UnityEngine;
using Component = ECSModel.Component;

public class TileOperationCom : Component
{
    Dictionary<int, SingleTileData> thisWaveTileData = null;
    private TilesAvatarComponent mTileAvatar;
    private JsonLibComponent jsonlib;
    private int thisWaveId;
    private InGameDataCom inGameData;
    
    public void Init(TilesAvatarComponent avatar,int waveId,   List<LevelConfigData> thisWaveTiles )
    {
        mTileAvatar = avatar;
        InitTmpData(waveId);
        InitThisWaveData(waveId,thisWaveTiles);
    }

    void InitTmpData( int waveId )
    {
        jsonlib = Game.Scene.GetComponent<JsonLibComponent>();
        inGameData = MapComponent.Inst.CurMap.GetComponent<InGameDataCom>();
        damageLv1 = jsonlib.GetDamageLv1();
        damageLv2 = jsonlib.GetDamageLv2();
        thisWaveId = waveId;
    }

    void InitThisWaveData(int waveId,List<LevelConfigData> thisWaveData )
    {
        thisWaveTileData = new Dictionary<int, SingleTileData>();
        foreach (var VARIABLE in thisWaveData)
        {
            CreateSingleTileData(VARIABLE);
            
            if (VARIABLE.HP != 0)
                inGameData.OnAddTileData(waveId,VARIABLE.ID);
        }
    }
    
    void CreateSingleTileData( LevelConfigData data)
    {
        SingleTileData tile = new SingleTileData
        {
            ID = data.ID,
            ConfigData = data,
            CurHp = data.HP,
            MaxHp = data.HP,
            TileDamageState = SingleTileDamageLv.Normal
        };

        if (thisWaveTileData.ContainsKey(data.ID) == false)
            thisWaveTileData.Add(data.ID, tile);
    }
    
    
    public bool UpdateTileHp(int id, float atk)
    {
        thisWaveTileData.TryGetValue(id, out var tileData);
        if (tileData == null)
        {
            Log.Error("Update Tile HP 有问题");
            return false;
        }
        
        if (tileData.CurHp <= 0)
            return true;

        int tmpHp =  Mathf.FloorToInt(tileData.CurHp-atk);
        if (tmpHp <= 0)
        {
            if (Define.IsDebug ==false)
            {
                tileData.TileDamageState = SingleTileDamageLv.Destory;
                tileData.CurHp = 0;
                mTileAvatar.ShowDestryTileEffect(id);    
            }
            
            Game.EventSystem.Run<int,int,int>(EventIdType.OnNotifyReduceTile,thisWaveId,id,tileData.ConfigData.Reward);
            return true;
        }
        
        tileData.CurHp = tmpHp;
        
        tileData.TileDamageState = GetTileGetHitLv(tileData.MaxHp, tmpHp);
        
        mTileAvatar.UpdateTileHp(id,tmpHp);
        switch(  tileData.TileDamageState)
        {
            case SingleTileDamageLv.Normal:
                break;
            case SingleTileDamageLv.DamageLv1:
                mTileAvatar.ChangeTileAvatar(id, tileData.ConfigData.BrokenRes1);
                break;
            case SingleTileDamageLv.DamageLv2:
                mTileAvatar.ChangeTileAvatar(id, tileData.ConfigData.BrokenRes2);
                break;
        }
        
        return false;
    }

    public void Debug_UpdateTileData(int id)
    {
        thisWaveTileData.TryGetValue(id, out var tileData);
        tileData.TileDamageState = SingleTileDamageLv.Destory;
        tileData.CurHp = 0;
        mTileAvatar.ShowDestryTileEffect(id);
    }
    
    
    float damageLv1 = 0,damageLv2=0;
    SingleTileDamageLv  GetTileGetHitLv( int maxhp, int resthp )
    {
        int lv1 = Mathf.FloorToInt((float)maxhp * damageLv1);
        int lv2 = Mathf.FloorToInt((float)maxhp * damageLv2);

        if (resthp >= lv1)
        {
            return SingleTileDamageLv.Normal;
        }
        else if (resthp < lv1 && resthp >= lv2)
        {
            return SingleTileDamageLv.DamageLv1;
        }
        else if (resthp < lv2 && resthp > 0)
        {
            return SingleTileDamageLv.DamageLv2;
        }
        return SingleTileDamageLv.NONE;
    }

    public override void Dispose()
    {
        thisWaveTileData.Clear();
        base.Dispose();
    }
}
