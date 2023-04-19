using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using MonogolyConfig;

namespace ECSModel
{
    public static class BallFactory
    {
        public static async UniTask<Ball> Create(long id,int configID, BallAttributeCom otherBall =null )
        {
            //  临时给的名字
            ResourcesComponent resCom = Game.Scene.GetComponent<ResourcesComponent>();
            await resCom.LoadBundleAsync("ball.unity3d");
            GameObject ballobj = (GameObject)resCom.GetAsset("ball.unity3d", "Ball");
            GameObject go = UnityEngine.Object.Instantiate(ballobj);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.name = id.ToString();

            Ball ball = ComponentFactory.CreateWithId<Ball, GameObject>(id, go);
            ball.UnitId = id;
            ball.ConfigID = configID;
            JsonLibComponent jsonlib = Game.Scene.GetComponent<JsonLibComponent>();
            BallBaseData baseData = jsonlib.GetBaseBallDataByID(configID);

            BallAttributeCom attrbute = ball.AddComponent<BallAttributeCom>();
            attrbute.Init(baseData,otherBall);
            ball.LocalScale = new Vector3(attrbute.BallSize, attrbute.BallSize);
            BallPostionCom ballPosition = ball.AddComponent<BallPostionCom>();
            ballPosition.Init(attrbute.BallSize);
 
            BallAvatarCom avatar = ball.AddComponent<BallAvatarCom>();
            // 初始化 外观，大小
            avatar.Init(baseData);
            
            return ball;
        }
    }
    
    
}

