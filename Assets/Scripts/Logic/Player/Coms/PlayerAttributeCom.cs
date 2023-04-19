using System;
using ECSModel;
using Kunpo;
using UnityEngine;
using Component = ECSModel.Component;

[Serializable]
public class PlayerAttributeCom :Component
{
  
    [SerializeField]
    public float BigNumberFloat { get; set; }
      
    [SerializeField]
    public int BigNumberDigit
    {
        get;
        set;
    }
    
    [SerializeField]
    private BigNumber money = null;

    public void InitMoney( float f ,int digit )
    {
        money = new BigNumber(f,digit);
    }
    
    [SerializeField]
    public  BigNumber Money
    {
        get => money;
        private set => money = value;
    }

    /// <summary>
    /// 加减 钱
    /// </summary>
    /// <param name="changeMoney"></param>
    /// <returns></returns>
    public BigNumber AddMoney(BigNumber changeMoney)
    {
        money += changeMoney;
        BigNumberFloat = money.f;
        BigNumberDigit = money.digit;
        return money;
    }

    public BigNumber ReduceMoney( BigNumber number )
    {
        money -= number;
        if( money <=0)
            money.SetToZero();
        
        BigNumberFloat = money.f;
        BigNumberDigit = money.digit;

        return money;
    }

    int diamond = 1000;
    [SerializeField]
    public int Diamond
    {
        get => diamond;
        set => diamond = value;
    }

    /// <summary>
    /// 加减钻石
    /// </summary>
    /// <param name="changeDiamond"></param>
    /// <returns></returns>
    public int UpdateDiamond( int changeDiamond)
    {
        diamond += changeDiamond;
        if (diamond < 0)
            diamond = 0;

        return diamond;
    }

    // 玩家的当前关卡
    int playerlevel=1;
    [SerializeField]
    public int PlayerCurLevel
    {
        get => playerlevel;
        set => playerlevel = value;
    }

    // 玩家最大关卡
    int playermaxLevel = 1;
    [SerializeField]
    public int PlayerMaxLevel
    {
        get => playermaxLevel;
        set => playermaxLevel = value;
    }


}
