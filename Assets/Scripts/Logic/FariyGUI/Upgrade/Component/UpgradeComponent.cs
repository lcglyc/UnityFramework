using FairyGUI;
using DG.Tweening;
using ECSModel;
using Kunpo;
using UnityEngine;
using Component = ECSModel.Component;


[ObjectSystem]
public class UpgradeComponentSystem : AwakeSystem<UpgradeComponent>
{
    public override void Awake(UpgradeComponent self)
    {
        self.Awake(self);
    }
}


public class UpgradeComponent : Component
{
    //按钮
    GButton upgrade_atk_btn, upgrade_spd_btn, leftArrow_btn, rightArrow_btn;
    GObject moveBtn;

    //动效
    Controller BallUnlock;
    Transition atkUp , spdUp;
    //小球强化等级相关
    GTextField atk_lv, spd_lv, title_upgrade_atk, title_upgrade_spd, title_ball_ability, title_ball_name;

    //小球属性
    GTextField atk_num, spd_num, size_num, ball_num;
    GTextField unlock_tips;
    GProgressBar upgrade_atk_bar, upgrade_spd_bar;
    FUI mMainGamePanel;
    JsonLibComponent jsonLibCom;

    Player CurPlayer;
    PlayerAttributeCom mCurPlayerAttirbute;
    Ball CurSelectedBall;
    GameObject BallComGo;

    string notEnoughGoldTip, maxLevelTip;

    int CurSelectedBallIndex= 0;
    float ballMaxDistance = 0.0f,ballStartPoint=0.0f;
    
    public void Awake(UpgradeComponent self)
    {
        mMainGamePanel = self.GetParent<FUI>();
        jsonLibCom = Game.Scene.GetComponent<JsonLibComponent>();
        InitUpgradeBtns();
    }
     
    public void Init()
    {
        CurPlayer = PlayerComponent.Instance.MyPlayer;
        mCurPlayerAttirbute = CurPlayer.GetComponent<PlayerAttributeCom>();
        BallComGo = BallComponent.Instance.GameObject;
        CurSelectedBall = BallComponent.Instance.CurBall;
        CurSelectedBallIndex = BallComponent.Instance.GetCurBallIndex();
        BallAttributeCom ballAttr = CurSelectedBall.GetComponent<BallAttributeCom>();

        // 显示& 分布球
        LayoutBall();
        ChangeBallOnInit();
        InitBallInfo(ballAttr);
    }

    void  InitBallInfo(BallAttributeCom ballAttr)
    {
        int ballUpgradeStageMaxLv = jsonLibCom.GetBallUpgradeStageMaxLv(CurSelectedBall.ConfigID, 1, ballAttr.AtkUpgradeLv);
        UpdateAttrNum(ballAttr.BallSize, ballAttr.BallAtk, ballAttr.BallNumber, ballAttr.BallSpd);
        UpdateAtkLvText(ballAttr.AtkUpgradeLv, ballAttr.AtkUpgradeStageProgress, ballUpgradeStageMaxLv);
        ballUpgradeStageMaxLv = jsonLibCom.GetBallUpgradeStageMaxLv(CurSelectedBall.ConfigID, 2, ballAttr.SpdUpgradeLv);
        UpdateSpdLvText(ballAttr.SpdUpgradeLv, ballAttr.SpdUpgradeStageProgress, ballUpgradeStageMaxLv);
        title_ball_ability.text = ballAttr.BallConfigData.Ability;
        title_ball_name.text = ballAttr.BallConfigData.BallName;

        InitGoldText();
    }


    void LayoutBall()
    {
        Ball[] allBall = BallComponent.Instance.GetAll();
        int index = 0;
        foreach( Ball ball in allBall )
        {
            if (ball == CurSelectedBall)
            {
                index++;
                continue;
            }
            ball.Visable = true;
            ball.GetComponent<BallPostionCom>().MoveToCenter(index);
            index++;
        }
        ballMaxDistance = (allBall.Length - 1) * Define.BallDistance * -1.0f;

        long id = BallComponent.Instance.CurBall.Id;
        Game.EventSystem.Run<long,bool>(EventIdType.UpdateBallAlpha,id,true);
    }

    void ChangeBallOnInit()
    {
        Ball[] balls = BallComponent.Instance.GetAll();
        CurSelectedBall = balls[CurSelectedBallIndex];
        float x = CurSelectedBallIndex * Define.BallDistance * -1.0f;
        BallComGo.transform.DOMoveX(x, 0.2f).OnComplete(() =>
        {
            CurSelectedBall.LocalPosition = new Vector3(0, -3, 0);
            CurSelectedBall.GetComponent<BallPostionCom>().MoveToCenter(CurSelectedBallIndex);
        });
    }


    void ChangeBall(int index)
    {
        Ball[] balls = BallComponent.Instance.GetAll();
        CurSelectedBall = balls[index];
        float x = index * Define.BallDistance * -1.0f;
        BallComGo.transform.DOMoveX(x, 0.2f);
        CurSelectedBallIndex = index;
        BallAttributeCom ballAttr = CurSelectedBall.GetComponent<BallAttributeCom>();

        if (ballAttr.IsUnlock)
        {
            BallUnlock.selectedPage = "UnLock";
            InitBallInfo(ballAttr);
        }
        else
        {
            title_ball_ability.text = ballAttr.BallConfigData.Ability;
            title_ball_name.text = ballAttr.BallConfigData.BallName;
            BallUnlock.selectedPage = "Lock";
            int unlockLv = jsonLibCom.GetBaseBallDataByID(CurSelectedBall.ConfigID).UnlockValue;
            unlock_tips.SetVar("0", unlockLv.ToString()).FlushVars();
        }
    }

    void HideOtherBall()
    {
        Ball[] allBall = BallComponent.Instance.GetAll();
        long curBallID = BallComponent.Instance.CurBall.Id;
        foreach (Ball ball in allBall)
        {
            if (ball.Id == curBallID)
                continue;
                
            ball.Position = Vector2.one * 10.0f;
            ball.Visable = false;
        }
    }


    #region   初始化控件
    void InitUpgradeBtns()
    {
        upgrade_atk_btn = FGUIHelper.GetButton("btn_upgrade_atk", mMainGamePanel, OnClickUpgradeAtk);
        upgrade_spd_btn = FGUIHelper.GetButton("btn_upgrade_spd", mMainGamePanel, OnClickUpgradeSpd);
        leftArrow_btn = FGUIHelper.GetButton("btn_left_arrow", mMainGamePanel, OnClickLeftArrow);
        rightArrow_btn = FGUIHelper.GetButton("btn_right_arrow", mMainGamePanel, OnClickRightArrow);
        atk_lv = FGUIHelper.GetTextField("text_upgrade_atk_lvnum", mMainGamePanel);
        spd_lv = FGUIHelper.GetTextField("text_upgrade_spd_lvnum", mMainGamePanel);
        atk_num = FGUIHelper.GetTextField("text_atk_num", mMainGamePanel);
        spd_num = FGUIHelper.GetTextField("text_spd_num", mMainGamePanel);
        size_num = FGUIHelper.GetTextField("text_size_num", mMainGamePanel);
        ball_num = FGUIHelper.GetTextField("text_ballnum_num", mMainGamePanel);
        title_ball_ability = FGUIHelper.GetTextField("title_ball_ability", mMainGamePanel);
        title_ball_name = FGUIHelper.GetTextField("title_ball_name", mMainGamePanel);
        title_upgrade_atk = FGUIHelper.GetTextField("title_upgrade_atk_bar", mMainGamePanel);
        title_upgrade_spd = FGUIHelper.GetTextField("title_upgrade_spd_bar",mMainGamePanel);
        upgrade_atk_bar = FGUIHelper.GetProgressBar("bar_upgrade_atk", mMainGamePanel);
        upgrade_spd_bar = FGUIHelper.GetProgressBar("bar_upgrade_spd", mMainGamePanel);
        moveBtn = mMainGamePanel.Get("button_touch_ball").GObject;
        FGUIHelper.SwipeGesture(moveBtn, OnSwipeMove, OnSwipeEnd);
        BallUnlock = mMainGamePanel.GetController("BallUnlock");
        atkUp = FGUIHelper.GetTransition("AtkUpgrade", mMainGamePanel);
        spdUp = FGUIHelper.GetTransition("SpdUpgrade", mMainGamePanel);
        unlock_tips = FGUIHelper.GetTextField("text_unlock_tips", mMainGamePanel);
        unlock_tips.text = jsonLibCom.GetLanguageInfoCN("Text_BallUnlock_Condition_1");
        notEnoughGoldTip = jsonLibCom.GetLanguageInfoCN("Text_Upgrade_Tips_1");
        maxLevelTip = jsonLibCom.GetLanguageInfoCN("Text_Upgrade_Tips_2");
    }
    #endregion

    #region 按钮点击事件
    void OnClickUpgradeAtk()
    {
        int upgradeMode = 1;
        var ballAttr = CurSelectedBall.GetComponent<BallAttributeCom>();
        MonogolyConfig.UpgradeData upgradeInfo;
        upgradeInfo = jsonLibCom.SearchBall(CurSelectedBall.ConfigID, upgradeMode, ballAttr.AtkUpgradeLv, ballAttr.AtkUpgradeStage);
        int ballUpgradeStageMaxLv = jsonLibCom.GetBallUpgradeStageMaxLv(CurSelectedBall.ConfigID, 1, ballAttr.AtkUpgradeLv);
        int maxLv = jsonLibCom.GetBallMaxLv(CurSelectedBall.ConfigID, upgradeMode);
        if (!CanUpgrade(upgradeMode, upgradeInfo, ballAttr, maxLv)) return;

        if (upgradeInfo.UpgradeType == 1) //普通升级
        {
            atkUp.Play();
            ballAttr.BallAtk = ballAttr.BallAtk + upgradeInfo.AttrValue;
            ballAttr.AtkUpgradeStageProgress++;
        }
        else if (upgradeInfo.UpgradeType == 2) // 进阶
        {
            ballAttr.AtkUpgradeStageProgress = 1;
            ballAttr.BallSize = ballAttr.BallSize + upgradeInfo.AttrValue;
            CurSelectedBall.LocalScale = new Vector3(ballAttr.BallSize, ballAttr.BallSize);
            UpgradeScaleBall(ballAttr);
            ballAttr.AtkUpgradeLv += 1;
            ballUpgradeStageMaxLv = jsonLibCom.GetBallUpgradeStageMaxLv(CurSelectedBall.ConfigID, upgradeMode, ballAttr.AtkUpgradeLv);
            upgrade_atk_bar.value = upgrade_atk_bar.max / ballUpgradeStageMaxLv;
        }
        ballAttr.AtkUpgradeStage += 1;
        
        upgradeInfo = jsonLibCom.SearchBall(CurSelectedBall.ConfigID, upgradeMode, ballAttr.AtkUpgradeLv, ballAttr.AtkUpgradeStage);
        upgrade_atk_btn.title = upgradeInfo.Cost.ToString();
        if (ballAttr.AtkUpgradeStage >= maxLv) upgrade_atk_btn.title = "已满级";

        ReducePlayerGold(new BigNumber(upgradeInfo.Cost));
        if (mCurPlayerAttirbute.Money < upgradeInfo.Cost) upgrade_atk_btn.titleColor = UnityEngine.Color.red;
        UpdateAtkLvText(ballAttr.AtkUpgradeLv, ballAttr.AtkUpgradeStageProgress, ballUpgradeStageMaxLv);
        UpdateAttrNum(ballAttr.BallSize, ballAttr.BallAtk, ballAttr.BallNumber, ballAttr.BallSpd);
    }
    void OnClickUpgradeSpd()
    {
        int upgradeMode = 2;
        var ballAttr = CurSelectedBall.GetComponent<BallAttributeCom>();
        MonogolyConfig.UpgradeData upgradeInfo;
        upgradeInfo = jsonLibCom.SearchBall(CurSelectedBall.ConfigID, upgradeMode, ballAttr.SpdUpgradeLv, ballAttr.SpdUpgradeStage);
        int ballUpgradeStageMaxLv = jsonLibCom.GetBallUpgradeStageMaxLv(CurSelectedBall.ConfigID, 2, ballAttr.SpdUpgradeLv);
        int maxLv = jsonLibCom.GetBallMaxLv(CurSelectedBall.ConfigID, upgradeMode);
        if (!CanUpgrade(upgradeMode, upgradeInfo, ballAttr, maxLv)) return;

        if (upgradeInfo.UpgradeType == 1) //普通升级
        {
            ballAttr.SpdUpgradeStageProgress++;
            ballAttr.BallSpd = ballAttr.BallSpd + upgradeInfo.AttrValue;
            spdUp.Play();
        }
        else if (upgradeInfo.UpgradeType == 2) // 进阶
        {
            ballAttr.SpdUpgradeStageProgress = 1;
            ballAttr.BallNumber = ballAttr.BallNumber + (int)upgradeInfo.AttrValue;
            ballAttr.SpdUpgradeLv += 1;
            ballUpgradeStageMaxLv = jsonLibCom.GetBallUpgradeStageMaxLv(CurSelectedBall.ConfigID, upgradeMode, ballAttr.SpdUpgradeLv);
            upgrade_spd_bar.value = upgrade_spd_bar.max / ballUpgradeStageMaxLv;
        }
        ballAttr.SpdUpgradeStage += 1;

        upgradeInfo = jsonLibCom.SearchBall(CurSelectedBall.ConfigID, upgradeMode, ballAttr.SpdUpgradeLv, ballAttr.SpdUpgradeStage);
        upgrade_spd_btn.title = upgradeInfo.Cost.ToString();
        if (ballAttr.SpdUpgradeStage >= maxLv) upgrade_spd_btn.title = "已满级";

        ReducePlayerGold(new BigNumber(upgradeInfo.Cost));
        if (mCurPlayerAttirbute.Money < upgradeInfo.Cost) upgrade_spd_btn.titleColor = UnityEngine.Color.red;
        UpdateSpdLvText(ballAttr.SpdUpgradeLv, ballAttr.SpdUpgradeStageProgress, ballUpgradeStageMaxLv);
        UpdateAttrNum(ballAttr.BallSize, ballAttr.BallAtk, ballAttr.BallNumber, ballAttr.BallSpd);
    }

    //  这里X >0 表示 往左滑 <0 往右
    float moveToward = 0;
    float moveX = 0;
    void OnSwipeMove(EventContext context)
    {
        SwipeGesture gesture = (SwipeGesture)context.sender;
        float deltaX = gesture.delta.x;
        if (Mathf.Abs(deltaX) < 2) return;

        moveToward += deltaX;
        float x = BallComGo.transform.position.x + moveToward * Time.deltaTime;
        float y = BallComGo.transform.position.y;
        moveX = Mathf.Clamp(x, ballMaxDistance, ballStartPoint);
        BallComGo.transform.position = new Vector3(moveX, y, 0);
    }

    void OnSwipeEnd(EventContext context)
    {
        SwipeGesture gesture = (SwipeGesture)context.sender;
        float deltaX = gesture.delta.x;

        int selectedIndx = Mathf.CeilToInt(moveX / Define.BallDistance);
        moveX = selectedIndx * Define.BallDistance;
        float y = BallComGo.transform.position.y;
        BallComGo.transform.DOMove(new Vector3(moveX, y, 0), 0.2f);
        moveToward = 0;
        moveX = 0;

        ChangeBall(Mathf.Abs(selectedIndx));
    }

    void OnClickLeftArrow()
    {
        Ball[] balls  =BallComponent.Instance.GetAll();
        int ballIndex = CurSelectedBallIndex;
        ballIndex = (--ballIndex) <= 0 ? 0 : ballIndex;
        Log.Debug("ball index:" + ballIndex);
        ChangeBall(ballIndex);
    }

    void OnClickRightArrow()
    {
        Ball[] balls = BallComponent.Instance.GetAll();
        int max = balls.Length - 1;
        int ballIndex = CurSelectedBallIndex;
        ballIndex = (++ballIndex) >= max ? max : ballIndex;

        ChangeBall(ballIndex);
    }

    bool CanUpgrade(int upgradeMode, MonogolyConfig.UpgradeData upgradeInfo,BallAttributeCom ballAttr,int maxLv)
    {
        int stage = 0;
        if (upgradeMode == 1) stage = ballAttr.AtkUpgradeStage;
        else if(upgradeMode == 2) stage = ballAttr.SpdUpgradeStage;

        if (stage >= maxLv)
        {
            TipsComponent.Instance.ShowTips(maxLevelTip);
            return false;
        }
        if (mCurPlayerAttirbute.Money < upgradeInfo.Cost)
        {
            TipsComponent.Instance.ShowTips(notEnoughGoldTip);
            return false;
        }
        return true;
    }

    
    #endregion

    void ReducePlayerGold(BigNumber  num)
    {
        mCurPlayerAttirbute.ReduceMoney(num);
        Game.EventSystem.Run(EventIdType.UI_UpdatePlayerMoney);
    }
  
    void UpdateAtkLvText(int upgradeLv, int atkUpgradeStageProgress, int ballUpgradeStageMaxLv)
    {
        atk_lv.SetVar("0", upgradeLv.ToString()).FlushVars();
        title_upgrade_atk.SetVar("0", atkUpgradeStageProgress.ToString()).SetVar("1", ballUpgradeStageMaxLv.ToString()).FlushVars();
        upgrade_atk_bar.value = (upgrade_atk_bar.max / ballUpgradeStageMaxLv) * (atkUpgradeStageProgress);
    }

    void UpdateSpdLvText(int upgradeLv, int spdUpgradeStageProgress, int ballUpgradeStageMaxLv)
    {
        spd_lv.SetVar("0", upgradeLv.ToString()).FlushVars();
        title_upgrade_spd.SetVar("0", spdUpgradeStageProgress.ToString()).SetVar("1", ballUpgradeStageMaxLv.ToString()).FlushVars();
        upgrade_spd_bar.value = (upgrade_spd_bar.max / ballUpgradeStageMaxLv) * (spdUpgradeStageProgress);
    }

    void UpgradeScaleBall(BallAttributeCom attr)
    {
        var curBallTrans = CurSelectedBall.GameObject.transform;
        var ballScale = CurSelectedBall.LocalScale;
        var scaleSize = CurSelectedBall.LocalScale * 1.2f;
        curBallTrans.DOScale(scaleSize, 0.2f).onComplete = delegate ()
        { curBallTrans.DOScale(ballScale, 0.2f); };
    }
    void UpdateAttrNum(float size,float atk,float ballNum,float speed)
    {
        atk_num.text = atk.ToString("f2")+" k";
        spd_num.text = speed.ToString("f2")+" km/h";
        size_num.text = size.ToString("f2") +" m";
        ball_num.text = ballNum.ToString();
    }

    void InitGoldText()
    {
        BallAttributeCom ballAttr;
        Ball ball = BallComponent.Instance.CurBall;
        int maxLv = jsonLibCom.GetBallMaxLv(ball.ConfigID, 1);
        MonogolyConfig.UpgradeData upgradeInfo = GetUpgradeData(1,out ballAttr);
        upgrade_atk_btn.title = upgradeInfo.Cost.ToString();
        if (ballAttr.AtkUpgradeStage >= maxLv) upgrade_atk_btn.title = "已满级";
        else
        {
            if (mCurPlayerAttirbute.Money < upgradeInfo.Cost) upgrade_atk_btn.titleColor = UnityEngine.Color.red;
            else upgrade_atk_btn.titleColor = UnityEngine.Color.black;
        }

        maxLv = jsonLibCom.GetBallMaxLv(ball.ConfigID, 2);
        upgradeInfo = GetUpgradeData(2,out ballAttr);
        upgrade_spd_btn.title = upgradeInfo.Cost.ToString();
        if (ballAttr.SpdUpgradeStage >= maxLv) upgrade_spd_btn.title = "已满级";
        else
        {
            if (mCurPlayerAttirbute.Money < upgradeInfo.Cost) upgrade_spd_btn.titleColor = UnityEngine.Color.red;
            else upgrade_spd_btn.titleColor = UnityEngine.Color.black;
        }
    }

    MonogolyConfig.UpgradeData GetUpgradeData(int upgradeMode,out BallAttributeCom ballAttr)
    {
        Ball ball = BallComponent.Instance.CurBall;
        ballAttr = ball.GetComponent<BallAttributeCom>();
        MonogolyConfig.UpgradeData upgradeInfo;
        jsonLibCom.GetUpgradeInfo(ball.ConfigID, ballAttr.SpdUpgradeLv, ballAttr.SpdUpgradeStage, upgradeMode, ballAttr.SpdUpgradeType, out upgradeInfo);
        return upgradeInfo;
    }


    public void OnClosePanel()
    {
        if (CurSelectedBall.GetComponent<BallAttributeCom>().IsUnlock)
        {
            BallComponent.Instance.CurBall = CurSelectedBall;
        }

        HideOtherBall();
    }

}