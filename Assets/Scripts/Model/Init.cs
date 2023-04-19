using System;
using System.Threading;
using UnityEngine;

namespace ECSModel
{
    public class Init : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
            this.StartAsync().Coroutine();
        }

        private async ECSVoid StartAsync()
        {
            try
            {
                Debug.LogError("开始 运行");
                Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);
                DontDestroyOnLoad(gameObject);
                Debug.LogError("执行了 Game.EventSystem.Add");
                //  这里是基础类，不需要依赖其他组件
                Game.Scene.AddComponent<TimerComponent>();
                Game.Scene.AddComponent<ResourcesComponent>();
                Game.Scene.AddComponent<FUIPackageComponent>();
                Game.Scene.AddComponent<FUIComponent>();
                Game.Scene.AddComponent<TransDataCom>();
                // 序列化/反序列 组件，用来读取本地Player & Ball & Racket 的数据
                Game.Scene.AddComponent<SerializationComponent>();

                Game.Scene.AddComponent<GameCtrlComponent>();
                //玩家数据
                Game.Scene.AddComponent<PlayerComponent>();
                // 球
                Game.Scene.AddComponent<BallComponent>();
                // 挡板
                Game.Scene.AddComponent<RacketComponent>();

                // 添加地图组件，用来管理地图中的一些元素
                Game.Scene.AddComponent<MapComponent>();
                Game.Scene.AddComponent<WaveComponent>();

                //  这些是依赖了其他组件，比如jsonlib,是 依赖了resources，并且依赖的BundlerHelper
                Game.Scene.AddComponent<JsonLibComponent>();
                // 以上为基础初始化
                // 在init场景检查资源，初始化完成以后，要进入到主场景
                Game.EventSystem.Run(EventIdType.InitStarGameFUI);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void Update()
        {
            Game.EventSystem.Update();
        }

        private void LateUpdate()
        {
            Game.EventSystem.LateUpdate();
        }

        private void FixedUpdate()
        {
        }


        private void OnApplicationQuit()
        {
            Game.Close();
        }
    }
}