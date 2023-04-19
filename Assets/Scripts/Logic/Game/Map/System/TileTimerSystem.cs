using  ECSModel;

[Event(EventIdType.OnNotifyReduceTile)]
public class TileTimerSystem : AEvent<int,int,int>
{
    //  int a = wave, int b = TileID
    
    public override void Run(int waveId, int tileId,int money)
    {
        InGameDataCom mInGame = MapComponent.Inst.CurMap.GetComponent<InGameDataCom>();
        int waveTiles = mInGame.OnReduceTile(waveId,tileId);
        int line = ThisLevelTileLine();
        
        // 去增加钱
        mInGame.AddMoney(money);
        if (Define.IsDebug)
        {
            ForDebug(waveId,tileId);
        }
        
        
        if (waveTiles > line) return;
        
        int allRestNum = mInGame.GetRestTiles();
        if (CheMoveToNextWave(mInGame,waveId,allRestNum))
        {
            MoveToNextWave(waveId,mInGame.CurWave);
            return;
        }
        
        if (waveTiles != 0) return;
        
        if (allRestNum == 0) // 其他的关卡也没有砖块了,表示获胜
        {
            mInGame.IsSuccess = true;
            NotifyGameOver();
        }
        else // 总数不是0，表示有未消除的砖块，这关放弃
        {
            DisposeWaveEntitly(waveId);
        }
    }

    bool CheckLastWaveEmpty( int waveId, InGameDataCom tempData )
    {
        if (waveId == 1)
        {
            return true;
        }

        int lastWave = waveId - 1;
        int restTile =  tempData.GetThisWaveTiles(lastWave);

        return restTile == 0;
    }

    bool CheMoveToNextWave( InGameDataCom inGameData, int waveId, int allRestNum )
    {
        if (waveId == inGameData.MaxWave)
            return false;

        if (inGameData.CurWave == inGameData.MaxWave)
            return false;

        if (allRestNum == 0)
            return false;

        // 打中的是之前波次的砖块
        if (waveId < inGameData.CurWave)
        {
            int restTile = inGameData.GetThisWaveTiles(waveId);
            int curTile = inGameData.GetThisWaveTiles(inGameData.CurWave);
            if (restTile != 0 || curTile != 0)
                return false;
        }
        
        if (!CheckLastWaveEmpty(waveId,inGameData))
            return false;

        return true;
    }
    

    private void MoveToNextWave( int thisWaveId, int curWave )
    {
        // 前一关出发切换波次行为
        int nextWave = thisWaveId < curWave ? curWave : thisWaveId;
        Game.EventSystem.Run<int>(EventIdType.Move2NextWave, curWave );
    }

    private void NotifyGameOver()
    {
        Game.EventSystem.Run<GameState>(EventIdType.BattleResult, GameState.InGame_Reslut);
    }

    private int ThisLevelTileLine()
    {
        return 3;
    }

    private void DisposeWaveEntitly( int waveId )
    {
        WaveEntity entity= WaveComponent.Instance.GetByWaveID(waveId);
        if (entity == null)
        {
            Log.Debug("wave entity is null :" + waveId);
        }
        else
        {
            entity.Dispose();
        }
        
    }

    private void ForDebug( int waveId,int tileId )
    {
        WaveEntity entity = WaveComponent.Instance.GetByWaveID(waveId);
        TileOperationCom operation = entity.GetComponent<TileOperationCom>();
        operation.Debug_UpdateTileData(tileId);
    }
    
    
    
}


/*
 * 说明那一关，减少了ID是多少的的砖块
 */