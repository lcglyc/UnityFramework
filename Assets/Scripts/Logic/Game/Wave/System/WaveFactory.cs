using ECSModel;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MonogolyConfig;
using Object = UnityEngine.Object;

public static class WaveFactory
{
    public static async UniTask Create(int level, InGameDataCom InGameData)
    {
        JsonLibComponent json = Game.Scene.GetComponent<JsonLibComponent>();
        Dictionary<int, List<LevelConfigData>> thisLevelDatas;
        json.GetLevelConfigDatasByLevel(level, out thisLevelDatas);
        WaveComponent waveCom = Game.Scene.GetComponent<WaveComponent>();

        if (waveCom == null)
        {
            waveCom = Game.Scene.AddComponent<WaveComponent>();
        }

        LevelData thisLevelData = json.GetLevelDataByID(level);

        // 这里记录了 总共的波次
        InGameData.MaxWave = GetMaxWave(thisLevelDatas);

        Object baffleObj = await CreateBaffle();

        foreach (int id in thisLevelDatas.Keys)
        {
            GameObject go = new GameObject();
            WaveEntity entity = ComponentFactory.CreateWithId<WaveEntity, GameObject>(id, go);
            entity.WaveId = id;
            float x = thisLevelData.GridOffsetX;
            entity.Position = new Vector3(x, 0, 0);
            go.name = id.ToString();

            //LevelTilesComponent tiles = entity.AddComponent<LevelTilesComponent>();
            TileOperationCom operationCom = entity.AddComponent<TileOperationCom>();

            //每一关都有这个组件
            TilesAvatarComponent tileAvatar = entity.AddComponent<TilesAvatarComponent>();
            ChangeWaveComponent changeWave = entity.AddComponent<ChangeWaveComponent>();
            List<LevelConfigData> thisWave = thisLevelDatas[id];
            await tileAvatar.Init(thisWave);
            operationCom.Init(tileAvatar, id, thisWave);

            //await tiles.Init(thisWave,id);

            changeWave.Init(go);
            waveCom.Add(entity);

            if (id == 1)
                go.SetActive(true);
            else
            {
                // 第一个不需要挡板
                changeWave.CreateBaffle(baffleObj);
                go.transform.position = new Vector3(x, 3.0f, 0);
                go.transform.localScale = Vector3.one * 2.0f;
                go.SetActive(false);
            }


        }
    }

    static int GetMaxWave(Dictionary<int, List<LevelConfigData>> tmpData)
    {
        int maxIndex = 0;
        foreach (int key in tmpData.Keys)
        {
            if (key > maxIndex)
            {
                maxIndex = key;
            }
        }

        return maxIndex;
    }

    static async UniTask<Object> CreateBaffle()
    {
        string abName = "baffle.unity3d";
        string resName = "baffle";
        ResourcesComponent rescom = Game.Scene.GetComponent<ResourcesComponent>();
        await rescom.LoadBundleAsync(abName);
        return rescom.GetAsset(abName, resName);
    }

}
