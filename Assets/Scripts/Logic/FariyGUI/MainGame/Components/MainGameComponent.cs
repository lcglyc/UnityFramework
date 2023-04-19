using Cysharp.Threading.Tasks;
using FairyGUI;
using ECSModel;
using Kunpo;
using UnityEngine;
using Component = ECSModel.Component;

[ObjectSystem]
public class MainGameComponentSystem : AwakeSystem<MainGameComponent>
{
    public override void Awake(MainGameComponent self)
    {
        self.Awake(self);
    }
}
public class MainGameComponent : Component
{
    // 底部按钮
    GButton mMain_1, mMain_2, mMain_3, mMain_4;

    GButton mRank, btn_gm;

    // 中间数值
    GTextField mLevel_0_num, mLevel_1_num, mLevel_2_num;

    GTextField mGold_num, mCrys_num;

    //侧边栏
    GButton mFunc_btn, mFunc_1, mFunc_2, mFunc_3;

    // left level
    GObject mLeft_0_bg, mLeft_0_Boss, mLeftArrow;
    // right level
    GObject mRight_3_bg, mRight_3_Boss, mRightArrow;
    GObject mCenterBoss;

    // 动效
    Transition mLeftTranShow, mLeftTranHide, GameStart;
    bool IsShowLeftMennu = false;

    Player CurPlayer;
    PlayerAttributeCom mCurPlayerAttirbute;
    FUI mMainGamePanel;
    JsonLibComponent jsonlib;
    private TimerComponent timer;
    private SerializationComponent serializationComponent;
    public bool isChallengePanel = true;
    public void Awake(MainGameComponent self)
    {
        mMainGamePanel = self.GetParent<FUI>();
        mMainGamePanel.GObject.asCom.MakeFullScreen();
        CurPlayer = PlayerComponent.Instance.MyPlayer;
        jsonlib = Game.Scene.GetComponent<JsonLibComponent>();
        timer = Game.Scene.GetComponent<TimerComponent>();
        serializationComponent = Game.Scene.GetComponent<SerializationComponent>();
        mCurPlayerAttirbute = CurPlayer.GetComponent<PlayerAttributeCom>();
        mMainGamePanel.GObject.asCom.fairyBatching = true;
        InitBottomBtns();
        InitCenterNums();
        InitLeftBtns();
        InitOtherControllers();
        InitData();
    }

    void InitData()
    {
        IsShowLeftMennu = false;
        // 显示数据
        UpdatePlayerAttirbute();
    }

    #region   初始化控件

    private Controller mainController;
    void InitBottomBtns()
    {
        mMain_1 = FGUIHelper.GetButton("main_1", mMainGamePanel, OnClickMain1);
        mMain_2 = FGUIHelper.GetButton("main_2", mMainGamePanel, OnClickMain2);
        mMain_3 = FGUIHelper.GetButton("main_3", mMainGamePanel, OnClickMain3);
        mMain_4 = FGUIHelper.GetButton("main_4", mMainGamePanel, OnClickMain4);

        mainController = FGUIHelper.GetController("BtnMainSelect", mMainGamePanel);
        // mMain_1.pageOption.controller = mainController;
        // mMain_2.pageOption.controller = mainController;
        // mMain_3.pageOption.controller = mainController;
        // mMain_4.pageOption.controller = mainController;

        mMain_1.changeStateOnClick = false;
        mMain_2.changeStateOnClick = false;
        mMain_3.changeStateOnClick = false;
        mMain_4.changeStateOnClick = false;
    }

    void InitLeftBtns()
    {
        mFunc_btn = FGUIHelper.GetButton("func_btn", mMainGamePanel, FuncBtn);
        mFunc_1 = FGUIHelper.GetButton("func_1", mMainGamePanel, Func_1);
        mFunc_2 = FGUIHelper.GetButton("func_2", mMainGamePanel, Func_2);
        mFunc_3 = FGUIHelper.GetButton("func_3", mMainGamePanel, Func_3);
        mLeftTranShow = FGUIHelper.GetTransition("FuncShow", mMainGamePanel);
        mLeftTranHide = FGUIHelper.GetTransition("FuncHide", mMainGamePanel);
        GameStart = FGUIHelper.GetTransition("GameStrat", mMainGamePanel);

    }

    void InitCenterNums()
    {
        mLevel_0_num = FGUIHelper.GetTextField("level_0_num", mMainGamePanel);
        mLevel_1_num = FGUIHelper.GetTextField("level_1_num", mMainGamePanel);
        mLevel_2_num = FGUIHelper.GetTextField("level_2_num", mMainGamePanel);

        mLeft_0_bg = mMainGamePanel.Get("level_0_bg").GObject;
        mLeft_0_Boss = mMainGamePanel.Get("level_0_boss").GObject;
        mRight_3_bg = mMainGamePanel.Get("level_2_bg").GObject;
        mRight_3_Boss = mMainGamePanel.Get("level_2_boss").GObject;
        mCenterBoss = mMainGamePanel.Get("level_1_boss").GObject;

        mRightArrow = mMainGamePanel.Get("arrow1").GObject;
        mLeftArrow = mMainGamePanel.Get("arrow2").GObject;
    }

    void InitOtherControllers()
    {
        mRank = FGUIHelper.GetButton("rank", mMainGamePanel, RankBtn);
        mGold_num = FGUIHelper.GetTextField("gold_num", mMainGamePanel);
        mCrys_num = FGUIHelper.GetTextField("crys_num", mMainGamePanel);

        btn_gm = FGUIHelper.GetButton("btn_gm", mMainGamePanel, OnClickGMBtn);

    }

    #endregion

    #region 按钮点击事件

    private bool isNeedWait = false;

    void OnClickMain1()
    {
        ChangeBottons(GameState.MAINPANEL);
    }

    void OnClickMain2()
    {

        ChangeBottons(GameState.UPGRADEPANEL);
    }

    void OnClickMain3()
    {
        ChangeBottons(GameState.MAINCAR);
    }

    void OnClickMain4()
    {
        TipsComponent.Instance.ShowTips("功能尚未开启");
    }

    public void ChangeBottons(GameState newState)
    {
        if (isNeedWait)
            return;

        GameState oldState = GameCtrlComponent.Instance.CurGameState;
        if (newState == oldState) return;
        SendClearMsg(oldState, newState);
        GameCtrlComponent.Instance.CurGameState = newState;
        SendNewMsg(newState);
    }

    void SendClearMsg(GameState oldState, GameState newState)
    {
        // 清理老数据
        switch (oldState)
        {
            case GameState.MAINPANEL:
                {
                    isChallengePanel = false;
                }
                break;
            case GameState.UPGRADEPANEL:
                {
                    Game.EventSystem.Run<GameState>(EventIdType.CloseUpgradePanel, newState);
                }
                break;
            case GameState.MAINCAR:
                {
                    Game.EventSystem.Run(EventIdType.CloseRacketUpgradePanel);
                }
                break;
            case GameState.MAINPSTORE:
                {

                }
                break;
        }

    }

    void SendNewMsg(GameState newState)
    {
        switch (newState)
        {
            case GameState.MAINPANEL:
                {
                    isChallengePanel = true;
                    //       mMain_1.pageOption.index = 0;
                    mMain_1.selected = true;
                    SaveBallAttribute();
                    SaveRacketAttribute();
                }
                break;
            case GameState.UPGRADEPANEL:
                {
                    //  mMain_2.pageOption.name = "Main2";
                    mMain_2.selected = true;
                    Game.EventSystem.Run(EventIdType.InitUpgradePanel);

                    long id = RacketComponent.Instance.CurRacket.Id;
                    Game.EventSystem.Run<long, bool>(EventIdType.UpdateRacketAlpha, id, false);
                }
                break;
            case GameState.MAINCAR:
                {
                    //   mMain_3.pageOption.index = 2;
                    mMain_3.selected = true;
                    Game.EventSystem.Run(EventIdType.InitRacketUpgradePanel);

                    long id = BallComponent.Instance.CurBall.Id;
                    Game.EventSystem.Run<long, bool>(EventIdType.UpdateBallAlpha, id, false);
                }
                break;
            case GameState.MAINPSTORE:
                {
                    //    mMain_4.pageOption.index = 3;
                    mMain_4.selected = true;
                }
                break;
        }

        isNeedWait = true;
        AddTimers();
    }


    async UniTaskVoid AddTimers()
    {
        await timer.WaitAsync(500);
        isNeedWait = false;
    }


    void FuncBtn()
    {
        TipsComponent.Instance.ShowTips("功能尚未开启");
        return;

        if (IsShowLeftMennu)
            mLeftTranHide.Play();
        else
            mLeftTranShow.Play();

        IsShowLeftMennu = !IsShowLeftMennu;
    }

    void Func_1()
    {

    }

    void Func_2()
    {

    }

    void Func_3()
    {

    }

    void RankBtn()
    {
        TipsComponent.Instance.ShowTips("功能尚未开启");
    }

    void OnClickGMBtn()
    {
        if (GameCtrlComponent.Instance.CurGameState == GameState.GM)
            return;

        if (GameCtrlComponent.Instance.CurGameState != GameState.MAINPANEL)
        {
            TipsComponent.Instance.ShowTips("请在主界面打开 GM 功能");
            return;
        }



        Game.EventSystem.Run(EventIdType.OpenDebugPanel);
    }

    #endregion

    public void UpdatePlayerAttirbute()
    {
        UpdateMoneyUI(mCurPlayerAttirbute.Money);
        UpdateDiamondUI(mCurPlayerAttirbute.Diamond);
        UpdateLevelUI(mCurPlayerAttirbute.PlayerCurLevel);
    }

    public void UpdateMoneyUIByAttribute()
    {
        UpdateMoneyUI(mCurPlayerAttirbute.Money);
        SavePlayerAttribute();
    }

    public void UpdateDiamondUIByAttirbute()
    {
        UpdateDiamondUI(mCurPlayerAttirbute.Diamond);
        SavePlayerAttribute();
    }

    public void UpdateLevelUIByAttribute()
    {
        UpdateLevelUI(mCurPlayerAttirbute.PlayerCurLevel);
    }

    void UpdateMoneyUI(BigNumber money)
    {
        mGold_num.text = money.ToStringD3();
    }

    void UpdateDiamondUI(int newDiamod)
    {
        mCrys_num.text = newDiamod.ToString();
    }

    void UpdateLevelUI(int newLevel)
    {

        int last = newLevel - 1;
        mLevel_0_num.text = last.ToString();
        mLeft_0_Boss.visible = IsBossLevel(last);

        mLevel_1_num.text = newLevel.ToString();
        mCenterBoss.visible = IsBossLevel(newLevel);

        int next = newLevel + 1;
        mLevel_2_num.text = next.ToString();
        mRight_3_Boss.visible = IsBossLevel(next);

        CheckLevelState(newLevel);
        SavePlayerAttribute();
    }

    bool IsBossLevel(int level)
    {
        int lvType = jsonlib.GetLevelType(level);
        return lvType == 1;
    }

    public void CheckLevelState(int level)
    {

        mLeft_0_bg.visible = true;
        mLevel_0_num.visible = true;
        mLeftArrow.visible = true;
        mRight_3_bg.visible = true;
        mLevel_2_num.visible = true;
        mRightArrow.visible = true;

        if (level == 1)
        {
            mLeft_0_bg.visible = false;
            mLeft_0_Boss.visible = false;
            mLevel_0_num.visible = false;
            mLeftArrow.visible = false;
        }

        int maxLevel = 999; // 这里要读表
        if (level == maxLevel)
        {
            mRight_3_bg.visible = false;
            mRight_3_Boss.visible = false;
            mLevel_2_num.visible = false;
            mRightArrow.visible = false;
        }
    }


    public void UpdateGameMainPanelVisable(bool IsShow)
    {
        if (GameStart == null) return;

        if (IsShow)
        {
            mMainGamePanel.Visible = true;
            UpdatePlayerAttirbute();
            GameStart.PlayReverse();
            return;
        }

        GameStart.Play(
            () =>
            {
                mMainGamePanel.Visible = false;
            }
            );
    }


    //TODO:临时地方
    private void SavePlayerAttribute()
    {
        serializationComponent.SerializeationPlayerAttributeCom(mCurPlayerAttirbute);
    }

    private void SaveBallAttribute()
    {
        serializationComponent.SerializetionAllBall();
    }

    private void SaveRacketAttribute()
    {
        serializationComponent.SerializetionAllRacket();
    }
}