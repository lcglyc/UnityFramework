using System.Threading;

namespace ECSModel
{
    [ObjectSystem]
    public class ECSCancellationTaskSources: AwakeSystem<ECSCancellationTokenSource>
    {
        public override void Awake(ECSCancellationTokenSource self)
        {
            self.CancellationTokenSource = new CancellationTokenSource();
        }
    }
    
    [ObjectSystem]
    public class ETCancellationTokenSourceAwake2System: AwakeSystem<ECSCancellationTokenSource, long>
    {
        public override void Awake(ECSCancellationTokenSource self, long afterTimeCancel)
        {
            self.CancellationTokenSource = new CancellationTokenSource();
            self.CancelAfter(afterTimeCancel).Coroutine();
        }
    }
    
    public class ECSCancellationTokenSource: Component
    {
        public CancellationTokenSource CancellationTokenSource;

        public void Cancel()
        {
            this.CancellationTokenSource.Cancel();
            this.Dispose();
        }

        public async ECSVoid CancelAfter(long afterTimeCancel)
        {
            await Game.Scene.GetComponent<TimerComponent>().WaitAsync(afterTimeCancel);
            this.CancellationTokenSource.Cancel();
            this.Dispose();
        }

        public CancellationToken Token
        {
            get
            {
                return this.CancellationTokenSource.Token;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            
            base.Dispose();
            
            this.CancellationTokenSource?.Dispose();
            this.CancellationTokenSource = null;
        }
    }
}