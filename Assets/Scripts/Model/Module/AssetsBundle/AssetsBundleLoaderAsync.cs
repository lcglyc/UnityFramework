using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ECSModel
{
    [ObjectSystem]
    public class AssetsBundleLoaderAsyncSystem : UpdateSystem<AssetsBundleLoaderAsync>
    {
        public override void Update(AssetsBundleLoaderAsync self)
        {
            self.Update();
        }
    }

    public class AssetsBundleLoaderAsync : Component
    {
        private AssetBundleCreateRequest request;

        
        private UniTaskCompletionSource<AssetBundle> tcs;

        public void Update()
        {
            if (!this.request.isDone)
            {
                return;
            }

            UniTaskCompletionSource<AssetBundle> t = tcs;
            t.TrySetResult(this.request.assetBundle);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
        }

        public UniTask<AssetBundle> LoadAsync(string path)
        {
            this.tcs = new UniTaskCompletionSource<AssetBundle>();
            this.request = AssetBundle.LoadFromFileAsync(path);
            return this.tcs.Task;
        }
    }
}
