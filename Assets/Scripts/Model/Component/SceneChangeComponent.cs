using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace ECSModel
{
    [ObjectSystem]
    public class SceneChangeComponentUpdateSystem : UpdateSystem<SceneChangeComponent>
    {
        public override void Update(SceneChangeComponent self)
        {
            if (self.loadMapOperation.isDone)
            {
                self.tcs.TrySetResult();
            }
        }
    }

    public class SceneChangeComponent : Component
    {
        public AsyncOperation loadMapOperation;
        public UniTaskCompletionSource tcs;
        public float deltaTime;
        public int lastProgress = 0;

        public UniTask ChangeSceneAsync(string sceneName)
        {
            this.tcs = new UniTaskCompletionSource();
            // 加载map
            this.loadMapOperation = SceneManager.LoadSceneAsync(sceneName);
            return this.tcs.Task;
        }

        public int Process
        {
            get
            {
                if (this.loadMapOperation == null)
                {
                    return 0;
                }
                return (int)(this.loadMapOperation.progress * 100);
            }
        }

        public void Finish()
        {
            this.tcs.TrySetResult();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.Entity.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.Entity.RemoveComponent<SceneChangeComponent>();
        }
    }
}