using Cysharp.Threading.Tasks;
using UnityEngine;
using MonogolyConfig;
namespace ECSModel
{
    public static class RacketFactory
    {
        public static async UniTask<Racket> Create(long id, int configID, RacketAttributeCom other = null)
        {
            RacketComponent rackets = Game.Scene.GetComponent<RacketComponent>();
            ResourcesComponent resCom = Game.Scene.GetComponent<ResourcesComponent>();
            var jsonLib = Game.Scene.GetComponent<JsonLibComponent>();
            await resCom.LoadBundleAsync("racket.unity3d");
            GameObject ballobj = (GameObject)resCom.GetAsset("racket.unity3d", "Racket");
            GameObject go = UnityEngine.Object.Instantiate(ballobj);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.name = id.ToString();

            Racket racket = ComponentFactory.CreateWithId<Racket, GameObject>(id, go);
            racket.ConfigID = configID;
            //  用来控制横版的移动和位置
            racket.AddComponent<RacketMoveCom>();

            var racketAttr = racket.AddComponent<RacketAttributeCom>();
            BoardBaseData boardBaseData = jsonLib.GetBaseRacketDataByID(configID);
            racketAttr.Init(boardBaseData, other);
            racket.NineSliceScale = racketAttr.Length;
            var shootingCom = racket.AddComponent<RacketShootingCom>();
            var fireRate = racketAttr.WeaponValue / 1000;
            shootingCom.Init(fireRate);

            var racketPosCom = racket.AddComponent<RacketPosCom>();
            racketPosCom.Init();
            rackets.Add(racket);

            return racket;
        }
    }
}