using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ECSModel;
using MonogolyConfig;
using UnityEngine;
using DG.Tweening;
using Component = ECSModel.Component;


// load sprite and prefab include tile,particle, and so on
public class TilesAvatarComponent : Component
{
    List<Sprite> thisLevelRes = null;
    ResourcesComponent resCom;
    GameObject bricksElement, particle = null;
    private TimerComponent timer;

    private Dictionary<int, GameObject> mTileGos = null;

    public async UniTask Init(List<LevelConfigData> tarTile)
    {
        resCom = Game.Scene.GetComponent<ResourcesComponent>();
        timer = Game.Scene.GetComponent<TimerComponent>();
        thisLevelRes = new List<Sprite>();
        List<string> tileNames = GetSpriteName(tarTile);
        mTileGos = new Dictionary<int, GameObject>();

        particle = await LoadParticle();
        await LoadThiWaveRes(tileNames);
        await LoadBricks(tarTile);
    }


    async UniTask LoadThiWaveRes(List<string> resNames)
    {
        foreach (string res in resNames)
        {
            if (string.IsNullOrEmpty(res) || res == string.Empty || res == "")
            {
                Log.Error(" Load This Wave Res Error:" + res);
                continue;
            }
            Sprite sprite = await LoadSpriteRes(res);
            thisLevelRes.Add(sprite);
        }
    }

    async UniTask <Sprite> LoadSpriteRes(string name)
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


    Sprite GetBrickSprite(string name)
    {
        Sprite sprite = thisLevelRes.Find(x => x.name == name);
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

            tmp.Add(res.Trim());
            tmp.Add(BrokenRes1.Trim());
            tmp.Add(BrokenRes2.Trim());
        }

        return tmp;
    }


    async UniTask LoadBricks(List<LevelConfigData> levelTiles)
    {
        bricksElement = await LoadObject("bricks.unity3d", "Bricks");

        Vector3 position = Vector3.zero;
        int index = 0;
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
            else if (data.BrickType == 99) // 临时解决方案，99 表示会旋转boss
            {
                go = await LoadBOSS(data);
                BossRotationAnimation animation = go.GetComponentInChildren<BossRotationAnimation>();
                if (animation != null)
                {
                    animation.DelayTime = index * 0.15f;
                    index++;
                }
            }
            // 临时解决方案结束，后期Boss 的效果都移动到lua里

            if (go != null)
            {
                go.SetActive(true);
                mTileGos.Add(data.ID, go);
            }
        }
    }

    GameObject GetTileGo(int id)
    {
        if (mTileGos.ContainsKey(id))
        {
            return mTileGos[id];
        }

        return null;
    }


    async UniTask<GameObject> LoadObject(string abName, string prefabName)
    {
        await resCom.LoadBundleAsync(abName);
        GameObject go = resCom.GetAsset(abName, prefabName) as GameObject;
        return go;
    }


    private async UniTask<GameObject> LoadBOSS(LevelConfigData data)
    {
        string bossName = data.Prefab.Trim();
        if (string.IsNullOrEmpty(bossName))
        {
            Log.Error("加载boss 出问题，资源为空:");
            return null;
        }

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

        // TextMeshPro  pro = go.GetComponentInChildren<TMPro.TextMeshPro>();
        TileListener tileListener = go.GetComponent<TileListener>();
        if (tileListener == null)
        {
            tileListener = go.AddComponent<TileListener>();
        }

        if (data.HP == 0)
        {
            // if (pro != null)
            //     pro.text = string.Empty;

            tileListener.CurType = CollisionType.Bricks_Obj;
        }
        else
        {
            // if (pro !=null)
            //     pro.text = data.HP.ToString();

            tileListener.CurType = CollisionType.Bricks;
        }
        return go;
    }

    private GameObject LoadBricks(GameObject baseBricks, LevelConfigData data)
    {
        GameObject go = CloneGameObject(baseBricks, data);
        go.GetComponentInChildren<SpriteRenderer>().sprite = GetBrickSprite(data.Res);
        return go;
    }

    async UniTask<GameObject> LoadParticle()
    {
        await UniTask.CompletedTask;
        string abName = "explodeparticle.unity3d";
        string prefabName = "ExplodeParticle";
        particle = await LoadObject(abName, prefabName);
        particle = resCom.GetAsset(abName, prefabName) as GameObject;
        particle.SetActive(false);
        return particle;
    }

    public void UpdateTileHp(int id, int tmpHp)
    {
        GameObject go = GetTileGo(id);
        go.transform.DOShakeScale(0.1f, 0.1f);

        // TextMeshPro pro =  go.GetComponentInChildren<TextMeshPro>();
        // if (pro == null)
        //     return;
        // pro.text = tmpHp.ToString();

    }

    public void ChangeTileAvatar(int id, string damgeAvatar)
    {
        GameObject go = GetTileGo(id);
        SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
        foreach (Sprite sprite in thisLevelRes)
        {
            if (sprite.name.ToLower() == damgeAvatar.ToLower())
                renderer.sprite = sprite;
        }
    }

    public void ShowDestryTileEffect(int id)
    {
        GameObject go = GetTileGo(id);
        ShowPartilce(go);
        go.SetActive(false);
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

    public override void Dispose()
    {
        if (this.thisLevelRes != null)
        {
            thisLevelRes.Clear();
        }
        if (bricksElement != null)
        {
            bricksElement = null;
        }

        if (mTileGos != null)
        {
            foreach (var VARIABLE in mTileGos.Keys)
            {
                GameObject.Destroy(mTileGos[VARIABLE]);
            }

            mTileGos.Clear();
        }

        if (null != particle)
        {
            particle = null;
        }



        base.Dispose();
    }
}
