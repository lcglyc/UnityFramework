using System.Text.RegularExpressions;
using FairyGUI;
using ECSModel;
using Kunpo;
using UnityEngine;
using Component = ECSModel.Component;

[ObjectSystem]
public class DebugComponentSystem : AwakeSystem<DebugComponent>
{
    public override void Awake(DebugComponent self){
        self.Awake(self);
    }
}

public class DebugComponent : Component
{

    private GTextInput input_level,
        input_money,
        input_crys,
        input_ball,
        input_board,
        input_command;

    private GTextInput input_atk,
        input_board_atk;

    private GSlider slider_spd,
        slider_size,
        slider_num,
        slider_board_size,
        slider_board_spd;

    private GButton btn_setlevel,
        btn_addmoney,
        btn_addmoneymax,
        btn_addcrys,
        btn_addcrysmax,
        btn_unlockball,
        btn_unlockball_all,
        btn_unlockboard,
        btn_unlockboard_all,
        btn_ok,
        btn_addatk10x,
        btn_addatk,
        btn_spd_init,
        btn_size_init,
        btn_num_init,
        btn_board_addatk,
        btn_board_addatk10x,
        btn_board_size_init,
        btn_board_spd_init,
        btn_close,
        button_init;

    private Controller panelController;


    private FUI DebugFuiPanel;
    private PlayerAttributeCom attributeCom;
    private BallAttributeCom ballAttributeCom;
    private RacketAttributeCom racketComponent;
    private SerializationComponent serCom;

    public void Awake(DebugComponent self)
    {
        DebugFuiPanel = self.GetParent<FUI>();
   
        InitControllers();
        attributeCom = PlayerComponent.Instance.MyPlayer.GetComponent<PlayerAttributeCom>();
        ballAttributeCom = BallComponent.Instance.CurBall.GetComponent<BallAttributeCom>();
        racketComponent = RacketComponent.Instance.CurRacket.GetComponent<RacketAttributeCom>();
        serCom = Game.Scene.GetComponent<SerializationComponent>();
        Stage.inst.onKeyDown.Add(OnKeyDown);
        //DebugFuiPanel.GObject.asCom.fairyBatching = true;
    }

    void InitControllers()
    {

        btn_close = FGUIHelper.GetButton("btn_close", DebugFuiPanel, OnClose);
        DebugFuiPanel.Get("tab_battle_temp");
        DebugFuiPanel.Get("tab_main");
        DebugFuiPanel.Get("bg");
        DebugFuiPanel.Get("bg_money");
        DebugFuiPanel.Get("text_money");
        DebugFuiPanel.Get("line_money");
        DebugFuiPanel.Get("line_crys");
        DebugFuiPanel.Get("bg_crys");
        DebugFuiPanel.Get("text_crys");
        
        DebugFuiPanel.Get("line_level");
        DebugFuiPanel.Get("bg_level");
        DebugFuiPanel.Get("text_level");
        
        DebugFuiPanel.Get("line_ball");
        DebugFuiPanel.Get("bg_ball");
        DebugFuiPanel.Get("text_ball");
        
        DebugFuiPanel.Get("line_board");
        DebugFuiPanel.Get("bg_board");
        DebugFuiPanel.Get("text_board");
        
        
        input_level = DebugFuiPanel.Get("input_level").GObject.asTextInput;
        input_money = DebugFuiPanel.Get("input_money").GObject.asTextInput;
        input_crys = DebugFuiPanel.Get("input_crys").GObject.asTextInput;
        input_ball = DebugFuiPanel.Get("input_ball").GObject.asTextInput;
        input_board = DebugFuiPanel.Get("input_board").GObject.asTextInput;
        input_command = DebugFuiPanel.Get("input_command").GObject.asTextInput;
        input_atk = DebugFuiPanel.Get("input_atk").GObject.asTextInput;
        input_board_atk = DebugFuiPanel.Get("input_board_atk").GObject.asTextInput;

        // Global btns
        btn_setlevel = FGUIHelper.GetButton("btn_setlevel", DebugFuiPanel, OnButtSetLevel);
        btn_addmoney = FGUIHelper.GetButton("btn_addmoney", DebugFuiPanel, OnButtonAddMoney);
        btn_addmoneymax = FGUIHelper.GetButton("btn_addmoneymax", DebugFuiPanel, OnButtonAddMoneyMax);
        btn_addcrys = FGUIHelper.GetButton("btn_addcrys", DebugFuiPanel, OnButtonAddCrys);
        btn_addcrysmax = FGUIHelper.GetButton("btn_addcrysmax", DebugFuiPanel, OnButtonAddCrysMax);
        btn_unlockball = FGUIHelper.GetButton("btn_unlockball", DebugFuiPanel, UnLockAllBall);
        btn_unlockball_all = FGUIHelper.GetButton("btn_unlockball_all", DebugFuiPanel, UnLockAllBall);
        btn_unlockboard = FGUIHelper.GetButton("btn_unlockboard", DebugFuiPanel, UnLockAllRacket);
        btn_unlockboard_all = FGUIHelper.GetButton("btn_unlockboard_all", DebugFuiPanel, UnLockAllRacket);
        btn_ok = FGUIHelper.GetButton("btn_ok", DebugFuiPanel, OnRunOrder);
        button_init = FGUIHelper.GetButton("button_init", DebugFuiPanel, OnInitUser);
        
       

        //第二分页
        slider_spd = FGUIHelper.GetSlider("slider_spd", DebugFuiPanel, OnSliderSpd);
        slider_size = FGUIHelper.GetSlider("slider_size", DebugFuiPanel, OnSliderSize);
        slider_num = FGUIHelper.GetSlider("slider_num", DebugFuiPanel, OnSliderNum);
        slider_board_size = FGUIHelper.GetSlider("slider_board_size", DebugFuiPanel, OnSliderBroadSize);
        slider_board_spd = FGUIHelper.GetSlider("slider_board_spd", DebugFuiPanel, OnSliderBroadSpd);

        btn_addatk = FGUIHelper.GetButton("btn_addatk", DebugFuiPanel, AddBallAtk);
        btn_addatk10x = FGUIHelper.GetButton("btn_addatk10x", DebugFuiPanel, AddBallAtkX10);
        btn_spd_init = FGUIHelper.GetButton("btn_spd_init", DebugFuiPanel, BallSpdInit);
        btn_size_init = FGUIHelper.GetButton("btn_size_init", DebugFuiPanel, BallSizeInit);
        btn_num_init = FGUIHelper.GetButton("btn_num_init", DebugFuiPanel, BallNumInit);
        btn_board_addatk = FGUIHelper.GetButton("btn_board_addatk", DebugFuiPanel, Board_addatk);
        btn_board_addatk10x = FGUIHelper.GetButton("btn_board_addatk10x", DebugFuiPanel, Board_addatkx10);
        btn_board_size_init = FGUIHelper.GetButton("btn_board_size_init", DebugFuiPanel, Board_size_init);
        btn_board_spd_init = FGUIHelper.GetButton("btn_board_spd_init", DebugFuiPanel, Board_spd_init);
        
    }


    public void OnInit()
    {
        input_level.text = input_money.text =
            input_crys.text = input_ball.text = input_board.text = input_command.text = string.Empty;
        input_command.RequestFocus();

    }

    #region  处理按钮

    void OnInitUser()
    {
        TipsComponent.Instance.ShowTips("清理完毕，重新进入游戏后生效");
        serCom.ClearAll();
    }

    void OnClose()
    {
        DebugFuiPanel.Visible = false;
        GameCtrlComponent.Instance.CurGameState = GameState.MAINPANEL;
    }

    void OnRunOrder()
    {
        string order = input_command.text;
        if (string.IsNullOrEmpty(order))
            return;

        string[] orders = order.Split(' ');
        if (orders.Length == 0)
            return;

        string order_1 = orders[0];

    }

    void OnButtSetLevel()
    {
        int tarLevel = 0;
        if (checkValue(input_level.text, out tarLevel))
        {
            SetLevel(tarLevel);
        }
        TipsComponent.Instance.ShowTips("修改等级完毕："+tarLevel);
    }

    void OnButtonAddMoney()
    {
        int tarLevel = 0;
        if (checkValue(input_money.text, out tarLevel))
        {
            SetMoveny(tarLevel);
        }
        TipsComponent.Instance.ShowTips("修改金币完毕："+tarLevel);
    }

    void OnButtonAddMoneyMax()
    {
        int tarLevel = 999999999;
        {
            SetMoveny(tarLevel);
        }
    }

    void OnButtonAddCrys()
    {
        int tarLevel = 0;
        if (checkValue(input_crys.text, out tarLevel))
        {
            SetDimond(tarLevel);
        }
        TipsComponent.Instance.ShowTips("修改钻石完毕："+tarLevel);
    }

    void AddBallAtk()
    {

        int value = 0;
        if (checkValue(btn_addatk.text, out value))
        {
            AddBallAtk(value);
        }
    }

    void AddBallAtkX10()
    {
        float value = ballAttributeCom.BallAtk * 10;
        AddBallAtk(value);
    }
    
    

    void BallSpdInit()
    {
        float spd = ballAttributeCom.BallConfigData.BaseSpd;
        ballAttributeCom.BallSpd = spd;
        slider_spd.value = spd;
    }

    void BallSizeInit()
    {
        float spd = ballAttributeCom.BallConfigData.DefalutScale;
        ballAttributeCom.BallSize = spd;
        slider_size.value = spd;
    }

    void BallNumInit()
    {
        int spd = ballAttributeCom.BallNumber;
        ballAttributeCom.BallNumber = spd;
        slider_num.value = spd;
    }

    void Board_addatk()
    {
        int value = 0;
        if (checkValue(input_board_atk.text, out value))
        {
            AddRacket(value);
        }
    }
    
    void Board_addatkx10()
    {
        int value = Mathf.FloorToInt(racketComponent.Atk);
        value = value * 10;
        AddRacket(value);
    }

    void Board_size_init()
    {
        float value = racketComponent.BoardConfigData.DefalutScale;
        racketComponent.Atk = value;
        slider_board_size.value = value;//racketComponent.BoardConfigData.DefalutScale;
    }

    void Board_spd_init()
    {
        float value = racketComponent.BoardConfigData.BaseSpd;
        racketComponent.Spd= value;
        slider_board_spd.value = value;
    }

    void UnLockAllBall()
    {
        Ball[] balls = BallComponent.Instance.GetAll();
        foreach (var VARIABLE in balls)
        {
            UnlockBall(VARIABLE);
        }
    }

    void UnLockAllRacket()
    {
        Racket[] rackets = RacketComponent.Instance.GetAll();
        foreach (var VARIABLE in rackets)
        {
            UnlockRacket(VARIABLE);
        }
    }
    

    void Unwork()
    {
        TipsComponent.Instance.ShowTips("尚未开放");
    }

    void OnButtonAddCrysMax()
    {
        int tarLevel = 999999999;
        SetDimond(tarLevel);
    }


    #endregion

    #region slider

    private int ballSpd = 0;
    void OnSliderSpd(EventContext context)
    {
        GSlider slider = (GSlider) context.sender;
        Log.Debug("On slider spd :" + slider.value);
    }

    void OnSliderSize(EventContext context)
    {
        GSlider slider = (GSlider) context.sender;
        Log.Debug("On slider size :" + slider.value);
    }

    void OnSliderNum(EventContext context)
    {
        GSlider slider = (GSlider) context.sender;
        Log.Debug("On slider num :" + slider.value);
    }

    void OnSliderBroadSize( EventContext context )
    {
        GSlider slider = (GSlider) context.sender;
        Log.Debug("OnSliderBroadSize :" + slider.value);
    }

    void OnSliderBroadSpd(EventContext context )
    {
        GSlider slider = (GSlider) context.sender;
        Log.Debug("OnSliderBroadSpd :" + slider.value);
    }



#endregion
    

    #region 辅助类
    
    private void DebugTips1()
    {
        TipsComponent.Instance.ShowTips("输入框为空");
    }
    
    private void DebugTips2()
    {
        TipsComponent.Instance.ShowTips("不是数字");
    }

    bool checkValue(string value ,out int key)
    {
        key = 0;
        if (string.IsNullOrEmpty(value))
        {
            DebugTips1();
            return false;
        }

        if (int.TryParse(value, out key))
        {
            return true;
        }

        DebugTips2();
        return false;
    }

    #endregion

    
    
    #region 执行命令行

    void SetLevel( int targetLevel )
    {
        attributeCom.PlayerCurLevel = targetLevel;
        Game.EventSystem.Run(EventIdType.UI_UpdatePlayerLevel);
    }

    void SetMoveny(int money)
    {
        BigNumber big = new BigNumber(money,1);
        attributeCom.AddMoney(big);
        Game.EventSystem.Run(EventIdType.UI_UpdatePlayerMoney);
    }

    void UnlockBall(Ball ball)
    {
        ball.GetComponent<BallAttributeCom>().IsUnlock = true;
    }

    void UnlockRacket( Racket racket)
    {
        racket.GetComponent<RacketAttributeCom>().IsUnlock = true;
    }
    
    void SetDimond(int target)
    {
        attributeCom.UpdateDiamond(target);
        Game.EventSystem.Run(EventIdType.UI_UpdatePlayerDiamond);
    }

    void AddBallAtk( float value)
    {
        ballAttributeCom.BallAtk += value;
    }

    void AddRacket( int value)
    {
        racketComponent.Atk += value;
    }
    
    
    #endregion


    void OnKeyDown(EventContext context)
    {
        if (context.inputEvent.keyCode != KeyCode.Return)
            return;

        if (GameCtrlComponent.Instance.CurGameState != GameState.GM)
        {
            GameCtrlComponent.Instance.CurGameState = GameState.GM;
            DebugFuiPanel.Visible = true;
            this.OnInit();
        }
        else if ( GameCtrlComponent.Instance.CurGameState == GameState.GM)
        {
            bool isOrder = RegexName(input_command.text); 
           
            if (isOrder ==false && DebugFuiPanel.Visible == true)
            {
                OnClose();
            }
            else 
            {
                RunOrder(input_command.text);
            }
            
            
        }
    }
    
    void RunOrder( string order)
    {
        
    }
    
    public bool RegexName(string str)
    {
        bool flag= Regex.IsMatch(str,@"^[\u4e00-\u9fa5_a-zA-Z0-9]+$");
        return flag;
    }
    
}