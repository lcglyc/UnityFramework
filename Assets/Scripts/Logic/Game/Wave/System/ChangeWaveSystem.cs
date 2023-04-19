using System.Collections.Generic;
using UnityEngine;
using ECSModel;
using DG.Tweening;

[Event(EventIdType.Move2NextWave)]
public class ChangeWaveSystem : AEvent<int>
{
    public override void Run(int curWave)
    {
        InGameDataCom inGame = MapComponent.Inst.CurMap.GetComponent<InGameDataCom>();
        int maxWave = inGame.MaxWave;
        
        if (curWave == maxWave)
            return;
        
        int level = curWave + 1;
        if(level == inGame.CurWave) return;
        
        // 判定前一关是否清理干净
        if (!IsLastWaveClear(curWave))
            return;
        
        // 更改界面
        Game.EventSystem.Run<int, int>(EventIdType.UI_UpdateBattleWave, level, maxWave);
        
        //TODO: 这里是不是最后一关出效果待定。
        if (level == maxWave)
        {
            ShowEffect();
        }
        
        UpdateBallSize(inGame.CurLevelID, curWave,inGame);
        ShrinkCurWave(curWave);
        ShowNextWave( curWave,maxWave);
        inGame.CurWave = level;
    }
    

    bool IsLastWaveClear( int curWave )
    {
        if (curWave == 1)
            return true;

        int lastWave = curWave - 1;
        InGameDataCom inGameData = MapComponent.Inst.CurMap.GetComponent<InGameDataCom>();
        int restTiles = inGameData.GetThisWaveTiles(lastWave);
        
        return restTiles == 0;
    }

     void ShrinkCurWave( int curWave )
    {
        WaveEntity curEnity = WaveComponent.Instance.GetByWaveID(curWave);
        if (curEnity == null)
        {
            return;
        }
        ChangeWaveComponent waveComponent= curEnity.GetComponent<ChangeWaveComponent>();
        if (waveComponent == null)
        {
            return;
        }
        waveComponent .StartShrink();
    }

     void ShowNextWave( int curWave, int maxWave)
    {
        // 出现下一关
        int nextWave = curWave + 1;
        if (nextWave > maxWave)
            return;
            

        InGameDataCom gameCom =  MapComponent.Inst.CurMap.GetComponent<InGameDataCom>();
        gameCom.CurWave = nextWave;
        WaveEntity tarEntity = WaveComponent.Instance.GetByWaveID(nextWave);
        tarEntity.GameObject.SetActive(true);
        tarEntity.Position = new Vector3(0, 3.0f, 0);
        tarEntity.GetComponent<ChangeWaveComponent>().StartNormal();
    }

     void ShowEffect()
     {
         MapEffectCompoent com = MapComponent.Inst.CurMap.GetComponent<MapEffectCompoent>();
         if (com != null)
         {
            com.ShowEffect();
         }
     }

     // 更新球的大小
     void UpdateBallSize( int curLevel ,int wave,InGameDataCom inGameData)
     {
         JsonLibComponent jsonlib = Game.Scene.GetComponent<JsonLibComponent>();
          float scale = jsonlib.GetThisLevelWaveBallScale( curLevel, wave);
         BallSplitCom splitCom = MapComponent.Inst.CurMap.GetComponent<BallSplitCom>();
         List<Ball> balls = splitCom.GetAllBalls();
         
         foreach (var VARIABLE in  balls )
         {
             Vector3 smallScale = Vector3.one * 0.1f;
             Vector3 ballScale = VARIABLE.LocalScale * scale;
             if (ballScale.x <= 0.1f)
             {
                 ballScale = smallScale;
             }

             inGameData.InGameBallScale = ballScale;
             VARIABLE.GameObject.transform.DOScale(ballScale,0.5f);
         }
     }

}
