using System;
using System.Threading;

namespace ECSModel
{
    public partial struct ECSTask
    {
        public static ECSTask CompletedTask => new ECSTask();

        public static ECSTask FromException(Exception ex)
        {
            ECSTaskCompletionSource tcs = new ECSTaskCompletionSource();
            tcs.TrySetException(ex);
            return tcs.Task;
        }

        public static ECSTask<T> FromException<T>(Exception ex)
        {
            var tcs = new ETTaskCompletionSource<T>();
            tcs.TrySetException(ex);
            return tcs.Task;
        }

        public static ECSTask<T> FromResult<T>(T value)
        {
            return new ECSTask<T>(value);
        }

        public static ECSTask FromCanceled()
        {
            return CanceledETTaskCache.Task;
        }

        public static ECSTask<T> FromCanceled<T>()
        {
            return CanceledETTaskCache<T>.Task;
        }

        public static ECSTask FromCanceled(CancellationToken token)
        {
            ECSTaskCompletionSource tcs = new ECSTaskCompletionSource();
            tcs.TrySetException(new OperationCanceledException(token));
            return tcs.Task;
        }

        public static ECSTask<T> FromCanceled<T>(CancellationToken token)
        {
            var tcs = new ETTaskCompletionSource<T>();
            tcs.TrySetException(new OperationCanceledException(token));
            return tcs.Task;
        }
        
        private static class CanceledETTaskCache
        {
            public static readonly ECSTask Task;

            static CanceledETTaskCache()
            {
                ECSTaskCompletionSource tcs = new ECSTaskCompletionSource();
                tcs.TrySetCanceled();
                Task = tcs.Task;
            }
        }

        private static class CanceledETTaskCache<T>
        {
            public static readonly ECSTask<T> Task;

            static CanceledETTaskCache()
            {
                var taskCompletionSource = new ETTaskCompletionSource<T>();
                taskCompletionSource.TrySetCanceled();
                Task = taskCompletionSource.Task;
            }
        }
    }

    internal static class CompletedTasks
    {
        public static readonly ECSTask<bool> True = ECSTask.FromResult(true);
        public static readonly ECSTask<bool> False = ECSTask.FromResult(false);
        public static readonly ECSTask<int> Zero = ECSTask.FromResult(0);
        public static readonly ECSTask<int> MinusOne = ECSTask.FromResult(-1);
        public static readonly ECSTask<int> One = ECSTask.FromResult(1);
    }
}