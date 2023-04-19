using MonogolyConfig;
using UnityEngine;
using Component = ECSModel.Component;
using ECSModel;

//  这里要处理游戏之外的属性变化，要保存在本地的
public class RacketAttributeCom : Component
{
    
    public void Init(BoardBaseData tmpboardBaseData,RacketAttributeCom otherRacket)
    {
        this.IsUnlock = false;
        this.baseBoardData = tmpboardBaseData;
        
        if (otherRacket == null)
        {
            InitRacketAsDefatult();
        }
        else
        {
            InitRacketAsOther(otherRacket);
        }
    }

    private BoardBaseData baseBoardData;

    public BoardBaseData BoardConfigData
    {
        get => baseBoardData;
    }

    private void InitRacketAsDefatult()
    {
        Atk = baseBoardData.BaseAtk;
        Spd = baseBoardData.BaseSpd;
        Length = baseBoardData.DefalutScale;
        AtkUpgradeLv = 1;
        LengthUpgradeLv = 1;
        AbilityUpgradeLv = 1;
        this.ConfigID = this.baseBoardData.ID;
        CurWeaponType = (WeaponType)baseBoardData.WeaponType;
        InitWeaponValue(baseBoardData.WeaponValue1);
    }

    private void InitRacketAsOther( RacketAttributeCom otherRacket )
    {
        this.ConfigID = otherRacket.ConfigID;
        this.IsUnlock = otherRacket.IsUnlock;
        this.Atk = otherRacket.Atk;
        this.Spd = otherRacket.Spd;
        this.Length = otherRacket.Length;
        this.AtkUpgradeLv = otherRacket.AtkUpgradeLv;
        this.AbilityUpgradeLv = otherRacket.AbilityUpgradeLv;
        this.LengthUpgradeLv = otherRacket.LengthUpgradeLv;

        CurWeaponType = (WeaponType) otherRacket.CurWeaponType;
        InitWeaponValue(otherRacket.WeaponValue);
    }
    
    void InitWeaponValue(float value)
    {
        switch (CurWeaponType)
        {
            case WeaponType.SINGLE:
                WeaponValue = value;
                break;
            default:
                break;
        }
    }

    
    [SerializeField]
    public int ConfigID { get; set; }

    // 是否已经解锁
    [SerializeField]
    public bool IsUnlock { get; set; }

    [SerializeField]
    public float Atk { get; set; }
    
    [SerializeField]
    public float Spd { get; set; }
    
    [SerializeField]
    public float Length { get; set; }
    
    [SerializeField]
    public int AtkUpgradeLv { get; set; }
    
    [SerializeField]
    public int AbilityUpgradeLv { get; set; }
    
    [SerializeField]
    public int LengthUpgradeLv { get; set; }

    [SerializeField]
    public WeaponType CurWeaponType { get; set; }
    [SerializeField]
    public float WeaponValue { get; set; }
}

