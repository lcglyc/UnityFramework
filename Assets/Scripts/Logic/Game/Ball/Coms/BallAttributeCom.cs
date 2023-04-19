using System;
using MonogolyConfig;
using UnityEngine;
using Component = ECSModel.Component;

[Serializable]
//  这里要处理游戏之外的属性变化，要保存在本地的
public class BallAttributeCom : Component
{
    public void Init(BallBaseData baseBallData,BallAttributeCom otherBall)
    {
        mBaseData = baseBallData;

        
        if (otherBall == null)
        {
            InitBallDefaultData(baseBallData);
        }
        else
        {
            InitBall(otherBall);
        }
    }

    public void InitBallDefaultData(BallBaseData baseBallData)
    {
        this.BallAtk = baseBallData.BaseAtk;
        this.BallSize = baseBallData.DefalutScale;
        this.BallSpd = baseBallData.BaseSpd;
        this.BallNumber = baseBallData.BallNum;
        this.AtkUpgradeLv = 0;
        this.AtkUpgradeStage = 1;
        this.SpdUpgradeLv = 0;
        this.SpdUpgradeStage = 1;
        this.AtkUpgradeType = 1;
        this.SpdUpgradeType = 1;
        this.IsUnlock = false;
        this.AtkUpgradeStageProgress = 1;
        this.SpdUpgradeStageProgress = 1;
        this.ConfigID = mBaseData.ID;
    }

    public void InitBall( BallAttributeCom otherBallData )
    {
        this.ConfigID = otherBallData.ConfigID;
        this.BallAtk = otherBallData.BallAtk;
        this.BallSize =otherBallData.BallSize;
        this.BallSpd = otherBallData.BallSpd;
        this.BallNumber = otherBallData.BallNumber;
        this.AtkUpgradeLv = otherBallData.AtkUpgradeLv;
        this.AtkUpgradeStage = otherBallData.AtkUpgradeStage;
        this.SpdUpgradeLv = otherBallData.SpdUpgradeLv;
        this.SpdUpgradeStage = otherBallData.SpdUpgradeStage;
        this.AtkUpgradeType = otherBallData.AtkUpgradeType;
        this.SpdUpgradeType = otherBallData.SpdUpgradeType;
        this.IsUnlock =otherBallData.IsUnlock;
        this.AtkUpgradeStageProgress = otherBallData.AtkUpgradeStageProgress;
        this.SpdUpgradeStageProgress = otherBallData.SpdUpgradeStageProgress;
    }
    

    private BallBaseData mBaseData;
    public BallBaseData BallConfigData
    {
        get => mBaseData;
        set => mBaseData = value;
    }
    
    [SerializeField]
    public int ConfigID { get; set; }
    // 是否已经解锁
    [SerializeField]
    public bool IsUnlock { get; set; }
    [SerializeField]
    public int AtkUpgradeLv { get; set; }
    [SerializeField]
    public int AtkUpgradeStage { get; set; }
    [SerializeField]
    public int AtkUpgradeStageProgress { get; set; }
    [SerializeField]
    public int SpdUpgradeLv { get; set; }
    [SerializeField]
    public int SpdUpgradeStage { get; set; }
    [SerializeField]
    public int SpdUpgradeStageProgress { get; set; }
    [SerializeField]
    public int AtkUpgradeType { get; set; }
    [SerializeField]
    public int SpdUpgradeType { get; set; }
    [SerializeField]
    public float BallAtk { get; set; }
    [SerializeField]
    public float BallSize { get; set; }
    [SerializeField]
    public float BallSpd { get; set; }
    [SerializeField]
    public int BallNumber { get; set; }
}
