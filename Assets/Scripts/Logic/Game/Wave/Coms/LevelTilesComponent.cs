using ECSModel;
using DG.Tweening;
using MonogolyConfig;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Component = ECSModel.Component;

public class LevelTilesComponents : Component
{
    ResourcesComponent resCom;
    TimerComponent timer = null;
    JsonLibComponent jsonlib = null;
    InGameDataCom mInGameData;
    List<Sprite> thisLevelRes = null;
    GameObject bricksElement, particle = null;
    Dictionary<int, SingleTileData> thisWaveTileData = null;

    //当前波次里的砖块数量，当前关卡的ID
    public int thisWaveTileNum = 0, thisWaveID = 0;

    float damageLv1 = 0, damageLv2 = 0;

    public async UniTask Init(List<LevelConfigData> tarTile, int waveID)
    {
        InitComs();
        InitLists();
        InitLevelData(tarTile, waveID);

        List<string> tileNames = GetSpriteName(tarTile);

        particle = await LoadParticle();
        await LoadThiWaveRes(tileNames);
        await LoadBricks(tarTile);
    }

    void InitComs()
    {
        resCom = Game.Scene.GetComponent<ResourcesComponent>();
        timer = Game.Scene.GetComponent<TimerComponent>();
        jsonlib = Game.Scene.GetComponent<JsonLibComponent>();
        mInGameData = MapComponent.Inst.CurMap.GetComponent<InGameDataCom>();
    }

    void InitLists()
    {
        thisLevelRes = new List<Sprite>();
        thisWaveTileData = new Dictionary<int, SingleTileData>();
    }

    void InitLevelData(List<LevelConfigData> allTiles, int waveID)
    {
        damageLv1 = jsonlib.GetDamageLv1();
        damageLv2 = jsonlib.GetDamageLv2();

        int maxNumber = allTiles.Count;
        int zeroHp = GetZeroHPTile(allTiles);
        thisWaveTileNum = maxNumber - zeroHp;
        thisWaveID = waveID;
        testMoveWave = false;
    }

    int GetZeroHPTile(List<LevelConfigData> allTiles)
    {
        int index = 0;
        foreach (var VARIABLE in allTiles)
        {
            if (VARIABLE.HP == 0)
            {
                index++;
            }
        }

        return index;
    }

    #region 加载资源

    async UniTask LoadThiWaveRes(List<string> resNames)
    {
        foreach (string res in resNames)
        {
            if (string.IsNullOrEmpty(res) || res == string.Empty || res == "")
            {
                Debug.LogError(" Load This Wave Res Error:" + res);
                continue;
            }
            Sprite sprite = await LoadSpriteRes(res);
            thisLevelRes.Add(sprite);
        }
    }

    async UniTask<Sprite> LoadSpriteRes(string name)
    {
        string resab = string.Format("{0}.unity3d", name.ToLower());

        await resCom.LoadBundleAsync(resab);
        Sprite sprite = null;
        if (!Define.IsAsync)
        {
            Texture2D tmp = resCom.GetAsset(resab, name) as Texture2D;
            sprite = Sprite.Create(tmp, new Rect(0, 0, tmp.width, tmp.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            sprite = (Sprite)resCom.GetAsset(resab, name) as Sprite;
        }
        sprite.name = name;
        return sprite;
    }

    List<string> GetSpriteName(List<LevelConfigData> tiles)
    {
        List<string> tmp = new List<string>();
        foreach (LevelConfigData data in tiles)
        {
            string res = data.Res;
            string BrokenRes1 = data.BrokenRes1;
            string BrokenRes2 = data.BrokenRes2;

            string tmpRes = tmp.Find(x => x == res);
            string tmpRes1 = tmp.Find(x => x == BrokenRes1);
            string tmpRes2 = tmp.Find(x => x == BrokenRes2);

            if (tmpRes != null && tmpRes1 != null && tmpRes2 != null)
                continue;

            tmp.Add(res);
            tmp.Add(BrokenRes1);
            tmp.Add(BrokenRes2);
        }

        return tmp;
    }

    async UniTask<GameObject> LoadObject(string abName, string prefabName)
    {
        await resCom.LoadBundleAsync(abName);
        GameObject go = resCom.GetAsset(abName, prefabName) as GameObject;
        return go;
    }

    async UniTask LoadBricks(List<LevelConfigData> levelTiles)
    {
        bricksElement = await LoadObject("bricks.unity3d", "Bricks");

        Vector3 position = Vector3.zero;
        foreach (LevelConfigData data in levelTiles)
        {
            GameObject go = null;
            if (data.BrickType == 1) // 表示常规类型的砖块
            {
                go = LoadBricks(bricksElement, data);
            }
            else if (data.BrickType == 2)
            {
                go = await LoadBOSS(data);
            }

            CreateSingleTileData(go, data);
        }

        //  这里去做显示效果
        int bricksCount = thisWaveTileData.Count;
        int times = 1000 / bricksCount;
        foreach (SingleTileData tile in thisWaveTileData.Values)
        {
            tile.Go.SetActive(true);
            if (thisWaveID == 1)
                await timer.WaitAsync(times);
        }
    }


    private GameObject LoadBricks(GameObject baseBricks, LevelConfigData data)
    {
        GameObject go = CloneGameObject(baseBricks, data);
        go.GetComponentInChildren<SpriteRenderer>().sprite = GetBrickSprite(data.Res);
        return go;
    }

    private void CreateSingleTileData(GameObject go, LevelConfigData data)
    {
        SingleTileData tile = new SingleTileData();
        tile.ID = data.ID;
        tile.Go = go;
        tile.ConfigData = data;
        tile.CurHp = data.HP;
        tile.MaxHp = data.HP;
        tile.TileDamageState = SingleTileDamageLv.Normal;

        if (thisWaveTileData.ContainsKey(data.ID) == false)
            thisWaveTileData.Add(data.ID, tile);
    }


    // 类型是2boss

    private async UniTask<GameObject> LoadBOSS(LevelConfigData data)
    {
        string bossName = data.Prefab;
        string abName = string.Format("{0}.unity3d", bossName.ToLower());
        GameObject boss = await LoadObject(abName, bossName);
        return CloneGameObject(boss, data);
    }

    private GameObject CloneGameObject(GameObject target, LevelConfigData data)
    {
        Vector3 position = Vector3.zero;
        GameObject go = GameObject.Instantiate(target) as GameObject;
        go.SetActive(false);
        position.x = data.UnityPosX;
        position.y = data.UnityPosY;
        go.name = data.ID.ToString();
        go.transform.parent = this.GameObject.transform;
        go.transform.position = position;

        TileListener tileListener = go.GetComponent<TileListener>();
        if (tileListener == null)
        {
            tileListener = go.AddComponent<TileListener>();
        }

        if (data.HP == 0)
        {
            tileListener.CurType = CollisionType.Bricks_Obj;
        }
        else
        {
            tileListener.CurType = CollisionType.Bricks;
        }
        return go;
    }


    Sprite GetBrickSprite(string name)
    {
        Sprite sprite = thisLevelRes.Find(x => x.name == name);
        return sprite;
    }

    public async UniTask<GameObject> LoadParticle()
    {
        await UniTask.CompletedTask;
        string abName = "explodeparticle.unity3d";
        string prefabName = "ExplodeParticle";
        particle = await LoadObject(abName, prefabName);
        particle = resCom.GetAsset(abName, prefabName) as GameObject;
        particle.SetActive(false);
        return particle;
    }

    async UniTaskVoid ShowPartilce(GameObject go)
    {
        await UniTask.CompletedTask;
        GameObject tempParticle = GameObject.Instantiate(particle) as GameObject;
        tempParticle.transform.position = go.transform.position;
        tempParticle.SetActive(true);
        await timer.WaitAsync(1000);
        GameObject.Destroy(tempParticle);
    }

    #endregion


    #region  单个tile 属性修改


    private void ChangeTileAvatar(GameObject go, int damageLv, string damgeAvatar)
    {
        SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
        foreach (Sprite sprite in thisLevelRes)
        {
            if (sprite.name.ToLower() == damgeAvatar.ToLower())
                renderer.sprite = sprite;
        }
        go.transform.DOShakeScale(0.1f, 0.1f);
    }

    public bool UpdateTileHp(int id, int atk)
    {
        SingleTileData tileData;
        thisWaveTileData.TryGetValue(id, out tileData);
        if (tileData == null)
        {
            Log.Error("Update Tile HP 有问题");
            return false;
        }

        if (tileData.CurHp <= 0)
            return true;


        int tmpHp = GetDemage(tileData.CurHp, atk);
        if (tmpHp <= 0)
        {
            ShowDestryTileEffect(tileData);
            ReduceTile();
            return true;
        }

        tileData.TileDamageState = GetTileGetHitLv(tileData.MaxHp, tmpHp);
        switch (tileData.TileDamageState)
        {
            case SingleTileDamageLv.Normal:
                break;
            case SingleTileDamageLv.DamageLv1:
                ChangeTileAvatar(tileData.Go, (int)SingleTileDamageLv.DamageLv1, tileData.ConfigData.BrokenRes1);
                break;
            case SingleTileDamageLv.DamageLv2:
                ChangeTileAvatar(tileData.Go, (int)SingleTileDamageLv.DamageLv2, tileData.ConfigData.BrokenRes2);
                break;
        }



        tileData.CurHp = tmpHp;

        return false;
    }


    private void ShowDestryTileEffect(SingleTileData tileData)
    {
        mInGameData.AddMoney(tileData.ConfigData.Reward);
        tileData.TileDamageState = SingleTileDamageLv.Destory;
        tileData.CurHp = 0;
        tileData.Go.SetActive(false);
        ShowPartilce(tileData.Go);
    }

    private int GetDemage(int tileHP, int atk)
    {
        return tileHP - atk;
    }


    SingleTileDamageLv GetTileGetHitLv(int maxhp, int resthp)
    {
        int lv_1 = Mathf.FloorToInt((float)maxhp * damageLv1);
        int lv_2 = Mathf.FloorToInt((float)maxhp * damageLv2);

        if (resthp >= lv_1)
        {
            return SingleTileDamageLv.Normal;
        }
        else if (resthp < lv_1 && resthp >= lv_2)
        {
            return SingleTileDamageLv.DamageLv1;
        }
        else if (resthp < lv_2 && resthp > 0)
        {
            return SingleTileDamageLv.DamageLv2;
        }
        return SingleTileDamageLv.NONE;
    }


    bool testMoveWave = false;
    private void ReduceTile()
    {
        thisWaveTileNum--;

        if (thisWaveTileNum < 0)
            return;


        // 如果这关，不是最大关卡
        if (thisWaveID < mInGameData.MaxWave)
        {
            MoveNextWave();
        }
        else
        {
            // 如果这个ID 是最大的,并且计数为0
            if (thisWaveTileNum <= 0)
            {
                //mInGameData.AddPassdWave(thisWaveID);
                NotifyReult();
                this.GetParent<WaveEntity>().Dispose();
            }
        }
    }

    void MoveNextWave()
    {
        //        // 如果当前场景不是最后一个场景，走切换流程
        //        if (thisWaveID < mInGameData.MaxWave)
        //        {
        //            if (ShowNextWave())
        //            {
        //                testMoveWave = true;
        //                Game.EventSystem.Run<int>(EventIdType.Move2NextWave, thisWaveID);
        //            }
        //
        //            if (thisWaveTileNum == 0) {
        //                
        //                mInGameData.AddPassdWave(thisWaveID);
        //           
        //                if (mInGameData.PassAllWave())
        //                {
        //                    NotifyReult();
        //                }
        //                this.GetParent<WaveEntity>().Dispose();
        //            }
        //        }
    }

    void NotifyReult()
    {
        //         Log.Debug("Battle Result is start Running");
        //         if (mInGameData.PassAllWave())
        //         {
        //             Log.Debug("Battle Result is moving");
        //             Game.EventSystem.Run<GameState>(EventIdType.BattleResult, GameState.InGame_Reslut);
        //         }
        //             
        //         thisWaveTileNum = 0;
    }


    bool ShowNextWave()
    {
        //        if (thisWaveTileNum > 3)
        //            return false;
        //
        //        int nextWave = thisWaveID + 1;
        //        if (nextWave <= mInGameData.CurWave)
        //            return false;
        //
        //        // 如果下一关已经过了
        //        if (mInGameData.IsThisWavePassed(nextWave))
        //            return false;
        //            
        //
        //        if (testMoveWave) return false;
        //
        return true;
    }

    #endregion

    public override void Dispose()
    {

        if (this.thisLevelRes != null)
        {
            // todo: 卸载资源
            thisLevelRes.Clear();
        }
        if (bricksElement != null)
        {
            //resCom.UnloadBundle("bricks.unity3d");
            bricksElement = null;
        }

        if (thisWaveTileData != null)
        {
            foreach (SingleTileData tile in thisWaveTileData.Values)
            {
                MonoBehaviour.Destroy(tile.Go);
                tile.ConfigData = null;
            }

            thisWaveTileData.Clear();
        }

        thisWaveTileNum = 0;
        base.Dispose();
    }
}


