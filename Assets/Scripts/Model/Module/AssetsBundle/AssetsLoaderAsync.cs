using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace ECSModel
{
    [ObjectSystem]
    public class AssetsLoaderAsyncAwakeSystem : AwakeSystem<AssetsLoaderAsync, AssetBundle>
    {
        public override void Awake(AssetsLoaderAsync self, AssetBundle a)
        {
            self.Awake(a);
        }
    }

    [ObjectSystem]
    public class AssetsLoaderAsyncUpdateSystem : UpdateSystem<AssetsLoaderAsync>
    {
        public override void Update(AssetsLoaderAsync self)
        {
            self.Update();
        }
    }

    public class AssetsLoaderAsync : Component
    {
        private AssetBundle assetBundle;

        private AssetBundleRequest request;

        private UniTaskCompletionSource tcs;

        public void Awake(AssetBundle ab)
        {
            this.assetBundle = ab;
        }

        public void Update()
        {
            if (!this.request.isDone)
            {
                return;
            }

            UniTaskCompletionSource t = tcs;
            t.TrySetResult();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.assetBundle = null;
            this.request = null;
        }

        public async UniTask<UnityEngine.Object[]> LoadAllAssetsAsync()
        {
            await InnerLoadAllAssetsAsync();
            return this.request.allAssets;
        }

        private UniTask InnerLoadAllAssetsAsync()
        {
            this.tcs = new UniTaskCompletionSource();
            this.request = assetBundle.LoadAllAssetsAsync();
            return this.tcs.Task;
        }
    }
}
