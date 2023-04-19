using System.CodeDom;
using UnityEngine;
namespace ECSModel
{
	public static class EventIdType
	{
        // 显示Startgame界面
        public const string InitStarGameFUI = "InitStarGameFUI";
        public const string InitGameFinish = "InitGameFinish";

        public const string InitPlayer = "InitPlayer";
        public const string ChangePlayer = "ChangePlayer";

        public const string InitBattle = "InitBattle";
        public const string InitBattleOver = "InitBattleOver";// 这里的初始化结束，主要是指关卡加载完毕
        
        public const string UpdateMainGamePanelVisable = "UpdateMainGamePanelVisable";
        public const string UI_UpdatePlayerMoney = "UpdateUIPlayerMoney";// 刷新主界面钱的UI
        public const string UI_UpdatePlayerDiamond= "UpdateUIPlayerDiamond";// 刷新主界面钻石的UI
        public const string UI_UpdatePlayerLevel = "UpdateUIPlayherLevel";// 刷新主界面关卡的UI
        
        public const string InitUpgradePanel = "InitUpgradePanel"; //初始化养成面板
        public const string CloseUpgradePanel = "CloseUpgradePanel"; //关闭养成面板

        public const string InitRacketUpgradePanel = "InitRacketUpgradePanel";
        public const string CloseRacketUpgradePanel = "CloseRacketUpgradePanel";

        public const string OnBulletCollsionEnter = "OnBulletCollsionEnter";
        public const string OnBoxCollider2DEnter = "OnBoxCollider2DEnter";
        public const string OnBallOutFrame = "OnBallOutFrame";//   球碰到下边框
        public const string Move2NextWave = "Move2NextWave"; // 切换下一波
        public const string OnNotifyReduceTile = "OnNotifyReduceTile";// 减少了一个砖块
        public const string UsedDiamondRePlayer = "UsedDiamondRePlayer";//  使用 钻石重新玩

        public const string UpdateBallAlpha = "UpdateBallAlpha"; // 小球 显示/隐藏 ，主要配合UI界面切换
        public const string UpdateRacketAlpha = "UpdateRacketAlpha";// 地板 显示/隐藏，主要配合UI界面切换
        
        
        public const string BattleOver = "BattleOver";
        public const string BattleResult = "BattleResult";  // 战斗结算

        public const string UI_UpdateBattleTime = "UI_UpdateBattleTime";// 减少战斗界面的时间。
        public const string UI_UpdateBattleMoney = "UI_UpdateBattleMoney";// 刷新战斗界面的钱
        public const string UI_UpdateBattleWave = "UI_UpdateBattleWave";// 刷新战斗界面的波次进度
        
        // Debug
        public const string OpenDebugPanel = "OpenDebugPanel";//打开gm界面
        

        public const string LoginFinish = "LoginFinish";
        public const string SceneChange = "SceneChange";
        public const string NumbericChange = "NumbericChange";
        public const string LoadingBegin = "LoadingBegin";
        public const string LoadingFinish = "LoadingFinish";
        public const string EnterMapFinish = "EnterMapFinish";
        public const string LoadMapFinish = "LoadMapFinish";

    }

    public enum CollisionType
    {
        NONE,
        Racket,
        Bricks,
        Bricks_Obj,// 不会被摧毁的
        
    }

    public enum GameState
    {
        NONE = 0,
        MAINPANEL = 1,
        UPGRADEPANEL=2,
        MAINPSTORE=3,
        MAINCAR=4, //载具？？
        GM=5,
        INGAMEMAP = 6,
        InGame_Reslut =7,
        InGame_Defeat= 8
    }

    public enum SingleTileDamageLv
    {
        NONE,
        Normal,
        DamageLv1,
        DamageLv2,
        Destory
    }

    public enum WeaponType
    {
        SINGLE = 1
    }

    public class CollisionBridge
    {
        public int TileID;
        public int WaveID;
        public long BallID;
        public long BulletID;
        public CollisionType CollisionType;
        public ContactPoint2D HitPoint; 
    }

    public class SingleTileData
    {
        public int ID;
        public MonogolyConfig.LevelConfigData ConfigData;
        public GameObject Go;
        public SingleTileDamageLv TileDamageState;
        public int CurHp;
        public int MaxHp;
    }
}