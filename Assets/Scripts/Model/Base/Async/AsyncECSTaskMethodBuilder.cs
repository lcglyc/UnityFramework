using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;

namespace ECSModel
{
    public struct AsyncECSTaskMethodBuilder
    {
        private ECSTaskCompletionSource tcs;
        private Action moveNext;

        // 1. Static Create method.
        [DebuggerHidden]
        public static AsyncECSTaskMethodBuilder Create()
        {
            AsyncECSTaskMethodBuilder builder = new AsyncECSTaskMethodBuilder();
            return builder;
        }

        // 2. TaskLike Task property.
        [DebuggerHidden]
        public ECSTask Task
        {
            get
            {
                if (this.tcs != null)
                {
                    return this.tcs.Task;
                }

                if (moveNext == null)
                {
                    return ECSTask.CompletedTask;
                }

                this.tcs = new ECSTaskCompletionSource();
                return this.tcs.Task;
            }
        }

        // 3. SetException
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            if (this.tcs == null)
            {
                this.tcs = new ECSTaskCompletionSource();
            }

            if (exception is OperationCanceledException ex)
            {
                this.tcs.TrySetCanceled(ex);
            }
            else
            {
                this.tcs.TrySetException(exception);
            }
        }

        // 4. SetResult
        [DebuggerHidden]
        public void SetResult()
        {
            if (moveNext == null)
            {
            }
            else
            {
                if (this.tcs == null)
                {
                    this.tcs = new ECSTaskCompletionSource();
                }

                this.tcs.TrySetResult();
            }
        }

        // 5. AwaitOnCompleted
        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : INotifyCompletion
                where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                if (this.tcs == null)
                {
                    this.tcs = new ECSTaskCompletionSource(); // built future.
                }

                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.OnCompleted(moveNext);
        }

        // 6. AwaitUnsafeOnCompleted
        [DebuggerHidden]
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : ICriticalNotifyCompletion
                where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                if (this.tcs == null)
                {
                    this.tcs = new ECSTaskCompletionSource(); // built future.
                }

                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.UnsafeOnCompleted(moveNext);
        }

        // 7. Start
        [DebuggerHidden]
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        // 8. SetStateMachine
        [DebuggerHidden]
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }

    public struct ETAsyncTaskMethodBuilder<T>
    {
        private T result;
        private ETTaskCompletionSource<T> tcs;
        private Action moveNext;

        // 1. Static Create method.
        [DebuggerHidden]
        public static ETAsyncTaskMethodBuilder<T> Create()
        {
            var builder = new ETAsyncTaskMethodBuilder<T>();
            return builder;
        }

        // 2. TaskLike Task property.
        [DebuggerHidden]
        public ECSTask<T> Task
        {
            get
            {
                if (this.tcs != null)
                {
                    return new ECSTask<T>(this.tcs);
                }

                if (moveNext == null)
                {
                    return new ECSTask<T>(result);
                }

                this.tcs = new ETTaskCompletionSource<T>();
                return this.tcs.Task;
            }
        }

        // 3. SetException
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            if (this.tcs == null)
            {
                this.tcs = new ETTaskCompletionSource<T>();
            }

            if (exception is OperationCanceledException ex)
            {
                this.tcs.TrySetCanceled(ex);
            }
            else
            {
                this.tcs.TrySetException(exception);
            }
        }

        // 4. SetResult
        [DebuggerHidden]
        public void SetResult(T ret)
        {
            if (moveNext == null)
            {
                this.result = ret;
            }
            else
            {
                if (this.tcs == null)
                {
                    this.tcs = new ETTaskCompletionSource<T>();
                }

                this.tcs.TrySetResult(ret);
            }
        }

        // 5. AwaitOnCompleted
        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : INotifyCompletion
                where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                if (this.tcs == null)
                {
                    this.tcs = new ETTaskCompletionSource<T>(); // built future.
                }

                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.OnCompleted(moveNext);
        }

        // 6. AwaitUnsafeOnCompleted
        [DebuggerHidden]
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : ICriticalNotifyCompletion
                where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                if (this.tcs == null)
                {
                    this.tcs = new ETTaskCompletionSource<T>(); // built future.
                }

                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.UnsafeOnCompleted(moveNext);
        }

        // 7. Start
        [DebuggerHidden]
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        // 8. SetStateMachine
        [DebuggerHidden]
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}