using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using ECSModel;
using MonogolyConfig;

[ObjectSystem]
public class JsonLibComAwakeSystem : AwakeSystem<JsonLibComponent>
{
    public override void Awake(JsonLibComponent self)
    {
        self.Awake();
    }
}

public class JsonLibComponent : ECSModel.Component
{
    ResourcesComponent resCom;
    public void Awake()
    {
        resCom = Game.Scene.GetComponent<ResourcesComponent>();
        ReadConfigs();
    }

    void ReadConfigs()
    {
        ReadBallBaseData();
        ReadLevelData();
        ReadLevelLevelData();
        ReadConstantData();
        ReadBallUpgradeData();
        ReadBoardBaseData();
        ReadBoardUpgradeData();
        ReadLanguageData();
    }

    void ReadBallBaseData()
    {
        string bundleName = "ballbasedata.unity3d";
        resCom.LoadBundle(bundleName);
        TextAsset ta = resCom.GetAsset(bundleName, "BallBaseData") as TextAsset;
        MonogolyConfig.BallBaseDataManager.GetInstance().ReadData(ta.text);
        resCom.UnloadBundle(bundleName);
    }

    void ReadBallUpgradeData()
    {
        string bundleName = "upgradedata.unity3d";
        resCom.LoadBundle(bundleName);
        TextAsset ta = resCom.GetAsset(bundleName, "UpgradeData") as TextAsset;
        MonogolyConfig.UpgradeDataManager.GetInstance().ReadData(ta.text);
        resCom.UnloadBundle(bundleName);
        DistributeBallUpgread();
    }

    void ReadBoardBaseData()
    {
        string bundleName = "boardbasedata.unity3d";
        resCom.LoadBundle(bundleName);
        TextAsset ta = resCom.GetAsset(bundleName, "BoardBaseData") as TextAsset;
        MonogolyConfig.BoardBaseDataManager.GetInstance().ReadData(ta.text);
        resCom.UnloadBundle(bundleName);
    }
    public BoardBaseData GetBaseRacketDataByID(int ConfigID)
    {
        return BoardBaseDataManager.GetInstance().GetBoardBaseDataInfo(ConfigID);
    }

    void ReadBoardUpgradeData()
    {
        string bundleName = "boardupgradedata.unity3d";
        resCom.LoadBundle(bundleName);
        TextAsset ta = resCom.GetAsset(bundleName, "BoardUpgradeData") as TextAsset;
        MonogolyConfig.BoardUpgradeDataManager.GetInstance().ReadData(ta.text);
        resCom.UnloadBundle(bundleName);
    }

    void ReadLanguageData()
    {
        string bundleName = "language.unity3d";
        resCom.LoadBundle(bundleName);
        TextAsset ta = resCom.GetAsset(bundleName, "Language") as TextAsset;
        MonogolyConfig.LanguageManager.GetInstance().ReadData(ta.text);
        resCom.UnloadBundle(bundleName);
    }
    void ReadLevelData()
    {
        string bundleName = "leveldata.unity3d";
        resCom.LoadBundle(bundleName);
        TextAsset ta = resCom.GetAsset(bundleName, "LevelData") as TextAsset;
        MonogolyConfig.LevelDataManager.GetInstance().ReadData(ta.text);
        resCom.UnloadBundle(bundleName);
    }
 

    void ReadLevelLevelData()
    {
        string bundleName = "levelconfigdata.unity3d";
        resCom.LoadBundle(bundleName);
        TextAsset ta = resCom.GetAsset(bundleName, "LevelConfigData") as TextAsset;
        MonogolyConfig.LevelConfigDataManager.GetInstance().ReadData(ta.text);
        resCom.UnloadBundle(bundleName);
    }



    void ReadConstantData()
    {
        string bundleName = "constantdata.unity3d";
        resCom.LoadBundle(bundleName);
        TextAsset ta = resCom.GetAsset(bundleName, "ConstantData") as TextAsset;
        MonogolyConfig.ConstantDataManager.GetInstance().ReadData(ta.text);
        resCom.UnloadBundle(bundleName);
    }

    public BallBaseData GetBaseBallDataByID(int ballID) {
        return BallBaseDataManager.GetInstance().GetBallBaseDataInfo(ballID);
    }

    public int GetLevelType(  int levelID)
    {
        LevelData lvdata =  LevelDataManager.GetInstance().GetLevelDataInfo(levelID);
        if (lvdata == null) return 0;

        return lvdata.LevelType;
    }


    public int GetInGameReviveTime()
    {
        ConstantData cd = ConstantDataManager.GetInstance().GetConstantDataInfo("ReviveTime");
        string key = cd.ValueKey[0];
        int time = int.Parse(key);
        return time;
    }

    public int GetInGameReviveNeedCrys()
    {
        ConstantData cd = ConstantDataManager.GetInstance().GetConstantDataInfo("ReviveNeedCrys");
        string key = cd.ValueKey[0];
        int NeedCrys = int.Parse(key);
        return NeedCrys;
    }

    public string GetLanguageInfoCN(string key)
    {
        Language la = LanguageManager.GetInstance().GetLanguageInfo(key);
        string info = la.CN;

        return info;
    }

    public string GetLanguageInfoEN(string key)
    {
        Language la = LanguageManager.GetInstance().GetLanguageInfo(key);
        string info = la.EN;

        return info;
    }

    // 这个如果很高的话，根据ID排序以后，做区间。可以大幅度减少查询数量
    public void GetLevelConfigDatasByLevel(int level, out Dictionary<int, List<LevelConfigData>> curLeveldata)
    {
        Dictionary<int, LevelConfigData> mDicLevel = LevelConfigDataManager.GetInstance().GetConfigDic();
        curLeveldata = new Dictionary<int, List<LevelConfigData>>();
        foreach (LevelConfigData data in mDicLevel.Values)
        {
            if (data.LevelID != level) continue;

            int wave = data.Wave;

            if (!curLeveldata.ContainsKey(wave))
                curLeveldata.Add(wave, new List<LevelConfigData>());

            curLeveldata[wave].Add(data);
        }
    }

   public float GetThisLevelWaveBallScale( int level , int wave)
    {
        float value = 1.0f;
        LevelData data=  LevelDataManager.GetInstance().GetLevelDataInfo(level);
        if (data == null)
        {
            return value;
        }

        string[] msg = data.BallSizeParam.Split(',');

        int waveIndex = wave;
        string tmpValue = msg[waveIndex];

        if (!float.TryParse(tmpValue,out value))
        {
            return value;
        }
        
        return value;
    }


    public int GetLevelBossWave( int levelID )    
    {
        int bossWave = -1;
        Dictionary<int, List<LevelConfigData>> curLeveldata;
        GetLevelConfigDatasByLevel(levelID, out curLeveldata);
        if (curLeveldata == null || curLeveldata.Count == 0)
        {
            return bossWave;
        }

        foreach (var VARIABLE in curLeveldata.Keys)
        {
            List<LevelConfigData> tmpLevel = curLeveldata[VARIABLE];
            foreach (LevelConfigData configData in tmpLevel)
            {
                if (configData.WaveType == 2) // 2 = boss
                {
                    bossWave = VARIABLE;
                    break;
                }
            }
        }
        
        return bossWave;
    }
    

    public LevelData GetLevelDataByID( int level)
    {
         return  LevelDataManager.GetInstance().GetLevelDataInfo(level);
    }

    public int GetTileDrawer( int number)
    {
        ConstantData constandata = ConstantDataManager.GetInstance().GetConstantDataInfo("WaveChangeBricks_intlist");
        string[] values = constandata.ValueKey;
        int tmpReslut = 2;
        foreach( string value in values  )
        {
            string[] drawers = value.Split(',');
            if (drawers.Length != 3) { Debug.LogError("WaveChangeBricks_intlist  字段有问题"); return tmpReslut; }
            int min = int.Parse(drawers[0]);
            int max = int.Parse(drawers[0]);
            int result = int.Parse(drawers[0]);
            if( number >=min && number<= max)
            {
                tmpReslut = result;
                break;
            }
        }

        return tmpReslut;
    }

    public float GetDamageLv1()
    {
        return GetDamageByKey("BrickBroken1Cond");
    }

    public float GetDamageLv2()
    {
        return GetDamageByKey("BrickBroken2Cond");
    }

    public float GetChangeWaveDuraionTime()
    {
         return  GetDamageByKey("WaveChangeTime");
    }

    public string GetGameEffect(int level)
    {
        LevelData ld = LevelDataManager.GetInstance().GetLevelDataInfo(level);
        if (ld == null || string.IsNullOrEmpty(ld.CameraEffect) || string.IsNullOrWhiteSpace(ld.CameraEffect))
        {
            return string.Empty;
        }
        // 如果不

        return ld.CameraEffect;
    }
    
    private float GetDamageByKey( string key)
    {
        ConstantData constandata = ConstantDataManager.GetInstance().GetConstantDataInfo(key);
        string[] values = constandata.ValueKey;
        string targetValue = string.Empty;
        if (values.Length == 1)
        {
            targetValue = values[0];
        }

        return float.Parse(targetValue);
    }

    public void GetRacketUpgradeInfo(int configID, int upgradeMode, int upgradeLv,out BoardUpgradeData upgradeInfo)
    {
        Dictionary<int, BoardUpgradeData> mDicUpgrade = BoardUpgradeDataManager.GetInstance().GetConfigDic();
        upgradeInfo = new BoardUpgradeData();
        foreach (var data in mDicUpgrade.Values)
        {
            if (data.BoardID == configID && data.UpgradeMode == upgradeMode && data.UpgradeLv == upgradeLv)
            {
                upgradeInfo = data;
            }
        }
    }
    public int GetRacketMaxLv(int configID, int upgradeMode)
    {
        Dictionary<int, BoardUpgradeData> mDicUpgrade = BoardUpgradeDataManager.GetInstance().GetConfigDic();
        int maxLv = 0;
        foreach(var data in mDicUpgrade.Values)
        {
            if (data.BoardID == configID && data.UpgradeMode == upgradeMode)
            {
                if (data.UpgradeLv > maxLv)
                    maxLv = data.UpgradeLv;
            }
        }

        return maxLv;
    }

    public void GetUpgradeInfo(int configID,int upgradeLv,int upgradeStage, int upgrademode,int upgradeType,out UpgradeData upgradeInfo)
    {
        Dictionary<int, UpgradeData> mDicUpgrade = UpgradeDataManager.GetInstance().GetConfigDic();
        upgradeInfo = new UpgradeData();
        foreach (UpgradeData data in mDicUpgrade.Values)
        {
            if (data.BallID == configID && data.UpgradeLv == upgradeLv && data.UpgradeStage == upgradeStage
                && data.UpgradeMode == upgrademode && data.UpgradeType == upgradeType)
            {
                upgradeInfo = data;
            }
        }
    }

    
    //每个球的升级路线
    Dictionary<int, List<UpgradeData>> mBallUpgradeList= new Dictionary<int, List<UpgradeData>>();
    
    // 球 的大小等级划分
    public void DistributeBallUpgread()
    {
        Dictionary<int, UpgradeData> mDicUpgrade = UpgradeDataManager.GetInstance().GetConfigDic();

        foreach (UpgradeData data in mDicUpgrade.Values)
        {
            if (mBallUpgradeList.ContainsKey(data.BallID) == false)
                mBallUpgradeList.Add(data.BallID, new List<UpgradeData>());

            mBallUpgradeList[data.BallID].Add(data);
        }
    }

    public UpgradeData SearchBall( int ballID, int upgrademode,int bigLv,int smallLv )
    {
        List<UpgradeData> upgradeDatas;
        mBallUpgradeList.TryGetValue(ballID, out upgradeDatas);

        foreach( UpgradeData upgradeData in upgradeDatas )
        {
            if (upgradeData.UpgradeMode == upgrademode 
                && upgradeData.UpgradeLv == bigLv 
                && upgradeData.UpgradeStage == smallLv)

                return upgradeData;
        }

        return null;
    }

    public int GetBallMaxLv(int configId,int upgradeMode)
    {
        List<UpgradeData> upgradeDatas;
        mBallUpgradeList.TryGetValue(configId, out upgradeDatas);
        int maxLv = 1;
        foreach(var upgradeData in upgradeDatas)
        {
             if(upgradeData.UpgradeMode == upgradeMode)
             {
                if (upgradeData.UpgradeStage > maxLv) maxLv = upgradeData.UpgradeStage;
             }
        }
        
        return maxLv;
    }
    public int GetBallUpgradeStageMaxLv(int configId, int upgradeMode, int upgradeLv)
    {
        int maxLv = 0;
        List<UpgradeData> upgradeDatas;
        mBallUpgradeList.TryGetValue(configId, out upgradeDatas);
        foreach (var data in upgradeDatas)
        {
            if (data.UpgradeMode == upgradeMode && data.UpgradeLv == upgradeLv)
                maxLv++;
        }
        return maxLv;
    }
}
