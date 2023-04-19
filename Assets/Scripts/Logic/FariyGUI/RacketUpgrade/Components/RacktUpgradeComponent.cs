using FairyGUI;
using DG.Tweening;
using ECSModel;
using Kunpo;
using UnityEngine;
using Component = ECSModel.Component;
using MonogolyConfig;


[ObjectSystem]
public class RacketUpgradeAwakeSystem : AwakeSystem<RacketUpgradeComponent>
{
    public override void Awake(RacketUpgradeComponent self)
    {
        self.Awake(self);
    }
}

public class RacketUpgradeComponent : Component
{
    GButton upgrade_length_btn, weapon_atk_btn, weapon_ability_btn,left_arrow_btn,right_arrow_btn;
    GTextField length_num, weapon_atk, weapon_value1, weapon_value2, upgrade_length_lv, weapon_atk_lv, weapon_ability_lv,weapon_value1_title;
    GTextField title_board_name, title_board_ability, unlock_tips;
    GObject moveBtn;
    FUI mMainGamePanel;
    JsonLibComponent jsonLibCom;
    Controller RacketUnlock;
    Player CurPlayer;
    PlayerAttributeCom mCurPlayerAttribute;
    Racket CurSelectedRacket; 
    GameObject RacketComGo;
    Controller racketUnlock;
    string notEnoughGoldTip, maxLevelTip;

    JsonLibComponent jsonLib = Game.Scene.GetComponent<JsonLibComponent>();

    WeaponType curWeapon;
    int CurSelectRacketIndex = 0;
    float racketMaxDistance = 0.0f, racketStartPoint = 0.0f;
    public void Awake(RacketUpgradeComponent self)
    {
        mMainGamePanel = self.GetParent<FUI>();
        jsonLibCom = Game.Scene.GetComponent<JsonLibComponent>();
        CurPlayer = PlayerComponent.Instance.MyPlayer;
        mCurPlayerAttribute = CurPlayer.GetComponent<PlayerAttributeCom>();
        RacketComGo = RacketComponent.Instance.GameObject;
        
        InitBtns();
    }

    public void Init()
    {
        CurSelectedRacket = RacketComponent.Instance.CurRacket;
        CurSelectRacketIndex = RacketComponent.Instance.GetCurRacketIndex();
       
        RacketAttributeCom racketAttr = CurSelectedRacket.GetComponent<RacketAttributeCom>();
        BoardUpgradeData upgradeInfo = new BoardUpgradeData();
        curWeapon = (WeaponType)racketAttr.CurWeaponType;

        int upgradeMode = 1;
        int maxLv = jsonLib.GetRacketMaxLv(CurSelectedRacket.ConfigID, upgradeMode) +1;
        jsonLib.GetRacketUpgradeInfo(CurSelectedRacket.ConfigID, upgradeMode, racketAttr.LengthUpgradeLv, out upgradeInfo);
        title_board_name.text = racketAttr.BoardConfigData.BoardName;
        title_board_ability.text = racketAttr.BoardConfigData.Ability;

        upgrade_length_lv.SetVar("0", racketAttr.LengthUpgradeLv.ToString()).FlushVars();
        length_num.text = racketAttr.Length.ToString("f2") + " m";
        upgrade_length_btn.title = upgradeInfo.Cost.ToString();
        if (racketAttr.LengthUpgradeLv == maxLv) upgrade_length_btn.title = "已满级";
        if (mCurPlayerAttribute.Money < upgradeInfo.Cost) upgrade_length_btn.titleColor = UnityEngine.Color.red;
        else upgrade_length_btn.titleColor = UnityEngine.Color.black;


        upgradeMode = 2;
        maxLv = jsonLib.GetRacketMaxLv(CurSelectedRacket.ConfigID, upgradeMode) +1;
        jsonLib.GetRacketUpgradeInfo(CurSelectedRacket.ConfigID, upgradeMode, racketAttr.AtkUpgradeLv, out upgradeInfo);
        weapon_atk_lv.SetVar("0", racketAttr.AtkUpgradeLv.ToString()).FlushVars();
        weapon_atk.text = racketAttr.Atk.ToString();
        weapon_atk_btn.title = upgradeInfo.Cost.ToString();

        if (racketAttr.AtkUpgradeLv == maxLv) weapon_atk_btn.title = "已满级";
        if (mCurPlayerAttribute.Money < upgradeInfo.Cost) weapon_atk_btn.titleColor = UnityEngine.Color.red;
        else weapon_atk_btn.titleColor = UnityEngine.Color.black;

        upgradeMode = 3;
        maxLv = jsonLib.GetRacketMaxLv(CurSelectedRacket.ConfigID, upgradeMode) +1;
        jsonLib.GetRacketUpgradeInfo(CurSelectedRacket.ConfigID, upgradeMode, racketAttr.AbilityUpgradeLv, out upgradeInfo);
        weapon_ability_lv.SetVar("0", racketAttr.AbilityUpgradeLv.ToString()).FlushVars();
        weapon_value1_title.text = "射速: ";
        weapon_value1.text = racketAttr.WeaponValue.ToString();
        weapon_value1.text = CurSelectedRacket.GetComponent<RacketAttributeCom>().WeaponValue.ToString() + " ms";
        weapon_ability_btn.title = upgradeInfo.Cost.ToString();

        if (racketAttr.AbilityUpgradeLv == maxLv) weapon_ability_btn.title = "已满级";
        if (mCurPlayerAttribute.Money < upgradeInfo.Cost) weapon_ability_btn.titleColor = UnityEngine.Color.red;
        else weapon_ability_btn.titleColor = UnityEngine.Color.black;

        LayoutRacket();
        ChangeRacket(CurSelectRacketIndex);
        
    }

    void InitBtns()
    {
        upgrade_length_btn = FGUIHelper.GetButton("btn_upgrade_length", mMainGamePanel,OnClickLengthBtn);
        weapon_atk_btn = FGUIHelper.GetButton("btn_weapon_atk", mMainGamePanel,OnClickAtkBtn);
        weapon_ability_btn = FGUIHelper.GetButton("btn_weapon_ability", mMainGamePanel,OnClickAbilityBtn);
        left_arrow_btn = FGUIHelper.GetButton("btn_left_arrow_board", mMainGamePanel,OnClickLeftArrow);
        right_arrow_btn = FGUIHelper.GetButton("btn_right_arrow_board", mMainGamePanel,OnClickRightArrow);
        title_board_name = FGUIHelper.GetTextField("title_board_name", mMainGamePanel);
        title_board_ability = FGUIHelper.GetTextField("title_board_ability", mMainGamePanel);

        length_num = FGUIHelper.GetTextField("text_length_num", mMainGamePanel);
        weapon_atk = FGUIHelper.GetTextField("text_weapon_atk_num", mMainGamePanel);
        weapon_value1 = FGUIHelper.GetTextField("text_weapon_value1_num", mMainGamePanel);
        weapon_value2 = FGUIHelper.GetTextField("text_weapon_value2_num", mMainGamePanel);
        upgrade_length_lv = FGUIHelper.GetTextField("text_upgrade_length_lvnum", mMainGamePanel);
        weapon_atk_lv = FGUIHelper.GetTextField("text_weapon_atk_lvnum", mMainGamePanel);
        weapon_ability_lv = FGUIHelper.GetTextField("text_weapon_ability_lvnum", mMainGamePanel);
        weapon_value1_title = FGUIHelper.GetTextField("text_weapon_value1_title", mMainGamePanel);
        moveBtn = mMainGamePanel.Get("button_touch_board").GObject;
        racketUnlock = mMainGamePanel.GetController("BoardUnlock");
        FGUIHelper.SwipeGesture(moveBtn, OnSwipeMove, OnSwipeEnd);
        unlock_tips = FGUIHelper.GetTextField("text_unlock_board_tips",mMainGamePanel);
        unlock_tips.text = jsonLib.GetLanguageInfoCN("Text_BallUnlock_Condition_1");
        notEnoughGoldTip = jsonLibCom.GetLanguageInfoCN("Text_Upgrade_Tips_1");
        maxLevelTip = jsonLibCom.GetLanguageInfoCN("Text_Upgrade_Tips_2");

    }


    void LayoutRacket()
    {
        Racket[] allRacket = RacketComponent.Instance.GetAll();
        int index = 0;
        foreach(var racket in allRacket)
        {
            racket.Visible = true;
            racket.GetComponent<RacketPosCom>().MoveToCenter(index);
            index++;
        }
        racketMaxDistance = (allRacket.Length - 1) * Define.RacketDistance * -1;
        long id = RacketComponent.Instance.CurRacket.Id;
        Game.EventSystem.Run<long,bool>(EventIdType.UpdateRacketAlpha,id,true);
    }

    void HideOtherRacket()
    {
        Racket[] allRacket = RacketComponent.Instance.GetAll();
        long curBoardID = RacketComponent.Instance.CurRacket.Id;
        foreach (Racket racket  in allRacket)
        {
            if (racket.Id == curBoardID)
                continue;
            racket.Position = Vector2.one * 10.0f;
            racket.Visible = false;
        }
    }

    public void OnClosePanel()
    {
        if (CurSelectedRacket.GetComponent<RacketAttributeCom>().IsUnlock)
        {
            RacketComponent.Instance.CurRacket = CurSelectedRacket;
        }
        HideOtherRacket();  
    }

    void OnClickLengthBtn()
    {
        int upgradeMode = 1;
        RacketAttributeCom racketAttr = CurSelectedRacket.GetComponent<RacketAttributeCom>();
        BoardUpgradeData upgradeInfo = new BoardUpgradeData();
        int maxLv = jsonLib.GetRacketMaxLv(CurSelectedRacket.ConfigID, upgradeMode) +1;
        jsonLib.GetRacketUpgradeInfo(CurSelectedRacket.ConfigID, upgradeMode, racketAttr.LengthUpgradeLv, out upgradeInfo);
        if (racketAttr.LengthUpgradeLv >= maxLv)
        {
            TipsComponent.Instance.ShowTips(maxLevelTip);
            return;
        }
        if (mCurPlayerAttribute.Money < upgradeInfo.Cost)
        {
            TipsComponent.Instance.ShowTips(notEnoughGoldTip);
            return;
        }

        racketAttr.LengthUpgradeLv += 1;
        racketAttr.Length += upgradeInfo.AttrValue1;
        CurSelectedRacket.NineSliceScale = racketAttr.Length;
        upgrade_length_lv.SetVar("0", racketAttr.LengthUpgradeLv.ToString()).FlushVars();
        length_num.text = racketAttr.Length.ToString("f2") + " m";
        upgrade_length_btn.title = upgradeInfo.Cost.ToString();
        ReducePlayerGold(new BigNumber(upgradeInfo.Cost));

        if (racketAttr.LengthUpgradeLv == maxLv) upgrade_length_btn.title = "已满级";
        UpdateBtnText();
    }

    void OnClickAtkBtn()
    {
        int upgradeMode = 2;
        RacketAttributeCom racketAttr = CurSelectedRacket.GetComponent<RacketAttributeCom>();
        BoardUpgradeData upgradeInfo = new BoardUpgradeData();
        jsonLib.GetRacketUpgradeInfo(CurSelectedRacket.ConfigID, upgradeMode, racketAttr.AtkUpgradeLv, out upgradeInfo);
        int maxLv = jsonLib.GetRacketMaxLv(CurSelectedRacket.ConfigID, upgradeMode)+1;
        if (racketAttr.AtkUpgradeLv >= maxLv)
        {
            TipsComponent.Instance.ShowTips(maxLevelTip);
            return;
        }
        if (mCurPlayerAttribute.Money < upgradeInfo.Cost)
        {
            TipsComponent.Instance.ShowTips(notEnoughGoldTip);
            return;
        }


        racketAttr.AtkUpgradeLv += 1;
        racketAttr.Atk += upgradeInfo.AttrValue1;

        weapon_atk_lv.SetVar("0", racketAttr.AtkUpgradeLv.ToString()).FlushVars();
        weapon_atk.text = racketAttr.Atk.ToString();
        weapon_atk_btn.title = upgradeInfo.Cost.ToString();
        ReducePlayerGold(new BigNumber(upgradeInfo.Cost));

        if (racketAttr.AtkUpgradeLv == maxLv) weapon_atk_btn.title = "已满级";
        UpdateBtnText();
    }

    void OnClickAbilityBtn()
    {
        int upgradeMode = 3;
        RacketAttributeCom racketAttr = CurSelectedRacket.GetComponent<RacketAttributeCom>();
        BoardUpgradeData upgradeInfo = new BoardUpgradeData();
        int maxLv = jsonLib.GetRacketMaxLv(CurSelectedRacket.ConfigID, upgradeMode) + 1;
        jsonLib.GetRacketUpgradeInfo(CurSelectedRacket.ConfigID, upgradeMode, racketAttr.AbilityUpgradeLv, out upgradeInfo);
        if (racketAttr.AbilityUpgradeLv  >= maxLv)
        {
            TipsComponent.Instance.ShowTips(maxLevelTip);
            return;
        }
        if (mCurPlayerAttribute.Money < upgradeInfo.Cost)
        {
            TipsComponent.Instance.ShowTips(notEnoughGoldTip);
            return;
        }

        racketAttr.AbilityUpgradeLv += 1;
        weapon_ability_lv.SetVar("0", racketAttr.AbilityUpgradeLv.ToString()).FlushVars();
        UpdateWeaponValue(racketAttr, upgradeInfo);
        weapon_ability_btn.title = upgradeInfo.Cost.ToString();

        ReducePlayerGold(new BigNumber(upgradeInfo.Cost));

        if (racketAttr.AbilityUpgradeLv == maxLv) weapon_ability_btn.title = "已满级";
        UpdateBtnText();

    }

    float moveToward = 0;
    float moveX = 0;
    void OnSwipeMove(EventContext context)
    {
        SwipeGesture gesture = (SwipeGesture)context.sender;
        float deltaX = gesture.delta.x;
        if(Mathf.Abs(deltaX) <2)return;

        moveToward += deltaX;
        float x = RacketComGo.transform.position.x + moveToward * Time.deltaTime;
        float y = RacketComGo.transform.position.y;
        moveX = Mathf.Clamp(x , racketMaxDistance, racketStartPoint);
        RacketComGo.transform.position = new Vector3(moveX, y, 0);
    }
    
    void OnSwipeEnd(EventContext context)
    {
        SwipeGesture gesture = (SwipeGesture)context.sender;
        float deltaX = gesture.delta.x;

        int selectIndex = Mathf.CeilToInt(moveX / Define.RacketDistance);
        moveX = selectIndex * Define.RacketDistance;
        float y = RacketComGo.transform.position.y;
        RacketComGo.transform.DOMove(new Vector3(moveX, y, 0), 0.2f);
        moveToward = 0;
        moveX = 0;

        ChangeRacket(Mathf.Abs(selectIndex));
    }
    void OnClickLeftArrow()
    {
        Racket[] rackets = RacketComponent.Instance.GetAll();
        int racketIndex = CurSelectRacketIndex;
        racketIndex = (--racketIndex) < 0 ? 0 : racketIndex;
        ChangeRacket(racketIndex);
     
    }

    void OnClickRightArrow()
    {
        Racket[] rackets = RacketComponent.Instance.GetAll();
        int racketIndex = CurSelectRacketIndex;
        int max = rackets.Length - 1;
        racketIndex = (++racketIndex) > max ? max : racketIndex;
        ChangeRacket(racketIndex);
    }

    void UpdateBtnText()
    {
        BoardUpgradeData upgradeInfo = new BoardUpgradeData();
        RacketAttributeCom racketAttr = CurSelectedRacket.GetComponent<RacketAttributeCom>();

        jsonLib.GetRacketUpgradeInfo(CurSelectedRacket.ConfigID, 3, racketAttr.AbilityUpgradeLv, out upgradeInfo);
        if (mCurPlayerAttribute.Money < upgradeInfo.Cost) weapon_ability_btn.titleColor = UnityEngine.Color.red;

        jsonLib.GetRacketUpgradeInfo(CurSelectedRacket.ConfigID, 1, racketAttr.LengthUpgradeLv, out upgradeInfo);
        if (mCurPlayerAttribute.Money < upgradeInfo.Cost) upgrade_length_btn.titleColor = UnityEngine.Color.red;

        jsonLib.GetRacketUpgradeInfo(CurSelectedRacket.ConfigID, 2, racketAttr.AtkUpgradeLv, out upgradeInfo);
        if (mCurPlayerAttribute.Money < upgradeInfo.Cost) weapon_atk_btn.titleColor = UnityEngine.Color.red;

    }

    void ChangeRacket(int index)
    {
        Racket[] rackets = RacketComponent.Instance.GetAll();
        CurSelectRacketIndex = index;
        CurSelectedRacket = rackets[index];
        float x = index * Define.RacketDistance * -1f;
        RacketComGo.transform.DOMoveX(x, 0.2f);
        RacketAttributeCom racketAttr = CurSelectedRacket.GetComponent<RacketAttributeCom>();
        curWeapon = (WeaponType)racketAttr.CurWeaponType;
        if (racketAttr.IsUnlock)
        {
            racketUnlock.selectedPage = "Unlock";
        }
        else
        {
            title_board_name.text = racketAttr.BoardConfigData.BoardName;
            title_board_ability.text = racketAttr.BoardConfigData.Ability;
            racketUnlock.selectedPage = "Lock";

            int unlockLv = jsonLibCom.GetBaseRacketDataByID(CurSelectedRacket.ConfigID).UnlockValue;
            unlock_tips.SetVar("0", unlockLv.ToString()).FlushVars();
        }
    }

    void ReducePlayerGold(BigNumber num)
    {
        mCurPlayerAttribute.ReduceMoney(num);
        Game.EventSystem.Run(EventIdType.UI_UpdatePlayerMoney);
    }

    void UpdateWeaponValue(RacketAttributeCom attributeCom, BoardUpgradeData upgradeInfo)
    {
        switch (curWeapon)
        {
            case WeaponType.SINGLE:
                attributeCom.WeaponValue += upgradeInfo.AttrValue1;
                CurSelectedRacket.GetComponent<RacketShootingCom>().FireRateInitial = attributeCom.WeaponValue / 1000;
                weapon_value1.text = CurSelectedRacket.GetComponent<RacketAttributeCom>().WeaponValue.ToString() + " ms";
                break;
            default:
                break;
        }
    }
}