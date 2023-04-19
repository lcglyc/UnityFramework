using ECSModel;
using Kunpo;

[Event(EventIdType.UI_UpdateBattleTime)]
class OnUpdateBattleUISystem : AEvent
{
    public override void Run()
    {
        FUIComponent fuiCom= Game.Scene.GetComponent<FUIComponent>();
         FUI fui= fuiCom.Get(FUIType.BattlePanel);
        UIBattleComponent battle = fui.GetComponent<UIBattleComponent>();
        // 减少5秒
        battle.ReduceTime(5);
    }
}

[Event(EventIdType.UI_UpdateBattleMoney)]
class OnUpdateMoney : AEvent<BigNumber>
{
    public override void Run(BigNumber a)
    {
        FUIComponent fuiCom = Game.Scene.GetComponent<FUIComponent>();
        FUI fui = fuiCom.Get(FUIType.BattlePanel);
        UIBattleComponent battle = fui.GetComponent<UIBattleComponent>();
        battle.UpdateMoney(a);
    }
}


[Event(EventIdType.UI_UpdateBattleWave)] 
class OnUpdateBattleWave:AEvent<int,int>
{
    public override void Run(int a, int b)
    {
        FUIComponent fuiCom = Game.Scene.GetComponent<FUIComponent>();
        FUI fui = fuiCom.Get(FUIType.BattlePanel);
        UIBattleComponent battle = fui.GetComponent<UIBattleComponent>();
        battle.UpdateLevel(a, b);
        int bossWave = MapComponent.Inst.CurMap.GetComponent<InGameDataCom>().BossWave;
        if (bossWave != -1 && bossWave == a) battle.ShowBossWarning();
    }
}