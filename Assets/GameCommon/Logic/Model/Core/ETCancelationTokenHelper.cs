using Cysharp.Threading.Tasks;

namespace ET
{
    public static class ETCancelationTokenHelper
    {
        // public static async UniTask CancelAfter(this UniTaskCancellationExtensions self, long afterTimeCancel)
        // {
        //     if (self.IsCancel())
        //     {
        //         return;
        //     }
        //
        //     await TimerComponent.Instance.WaitAsync(afterTimeCancel);
        //
        //     if (self.IsCancel())
        //     {
        //         return;
        //     }
        //
        //     self.Cancel();
        // }
    }
}