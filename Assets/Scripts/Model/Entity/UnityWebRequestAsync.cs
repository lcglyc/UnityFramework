using System;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
namespace ECSModel
{
    [ObjectSystem]
    public class UnityWebRequestUpdateSystem : UpdateSystem<UnityWebRequestAsync>
    {
        public override void Update(UnityWebRequestAsync self)
        {
            self.Update();
        }
    }

    public class UnityWebRequestAsync : Component
    {
        public class AcceptAllCertificate : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }

        public static AcceptAllCertificate certificateHandler = new AcceptAllCertificate();

        public UnityWebRequest Request;

        public bool isCancel;

        public UniTaskCompletionSource tcs;

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.Request?.Dispose();
            this.Request = null;
            this.isCancel = false;
        }

        public float Progress
        {
            get
            {
                if (this.Request == null)
                {
                    return 0;
                }
                return this.Request.downloadProgress;
            }
        }

        public ulong ByteDownloaded
        {
            get
            {
                if (this.Request == null)
                {
                    return 0;
                }
                return this.Request.downloadedBytes;
            }
        }

        public void Update()
        {
            if (this.isCancel)
            {
                this.tcs.TrySetException(new Exception($"request error: {this.Request.error}"));
                return;
            }

            if (!this.Request.isDone)
            {
                return;
            }
            if (!string.IsNullOrEmpty(this.Request.error))
            {
                this.tcs.TrySetException(new Exception($"request error: {this.Request.error}"));
                return;
            }

            this.tcs.TrySetResult();
        }

        public UniTask DownloadAsync(string url)
        {
            this.tcs = new UniTaskCompletionSource();

            url = url.Replace(" ", "%20");
            this.Request = UnityWebRequest.Get(url);
            this.Request.certificateHandler = certificateHandler;
            this.Request.SendWebRequest();

            return this.tcs.Task;
        }
    }
}
