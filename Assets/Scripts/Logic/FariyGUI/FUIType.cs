
namespace ECSModel
{
    public static class FUIType
    {
        public const string LoginPackage = "StartGame";
        public const string LoginPanel = "StartMenu";
        public const string MainGamePackage = "MainGame";
        public const string MainGamePanel = "MainPanel";

        // 场景切换
        public const string TransLoadingPackage = "TransformScene";
        public const string TransLoadingPanel = "loading";

        // tips
        public const string WarningPanel = "WarningPanel";
        public const string TipsPanel = "CommonTips";
        public const string TipsPackage = "Tips";
        
        // battle
        public const string BattlePanel = "BattlePanel";

        // loading
        public const string LoadingPacage = "Loading";
        public const string LoadingPanel = "LoadingPanel";
        
        //skill
        public const string SkillPanel = "Skill";
        
        // event tips
        public const string EventTipsPanel = "Event";
        public const string DebugPanel = "DebugTools";

        public const string UI_BattleFailPanel = "FailPanel";//  战斗失败界面
        public const string UI_BattleResultPanel = "ResultPanel"; // 战斗结束结算
        
        //debug
        public const string UI_DebugPackage = "GM";
        public const string UI_DebugPanel = "GMMainPanel";
    }
}