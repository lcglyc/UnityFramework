﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ECSModel
{
    /// <summary>
    /// Lightweight unity specified task-like object.
    /// </summary>
    [AsyncMethodBuilder(typeof (AsyncECSTaskMethodBuilder))]
    public partial struct ECSTask: IEquatable<ECSTask>
    {
        private readonly IAwaiter awaiter;

        [DebuggerHidden]
        public ECSTask(IAwaiter awaiter)
        {
            this.awaiter = awaiter;
        }

        [DebuggerHidden]
        public AwaiterStatus Status => awaiter?.Status ?? AwaiterStatus.Succeeded;

        [DebuggerHidden]
        public bool IsCompleted => awaiter?.IsCompleted ?? true;

        [DebuggerHidden]
        public void GetResult()
        {
            if (awaiter != null)
            {
                awaiter.GetResult();
            }
        }
        
        public void Coroutine()
        {
        }

        [DebuggerHidden]
        public Awaiter GetAwaiter()
        {
            return new Awaiter(this);
        }

        public bool Equals(ECSTask other)
        {
            if (this.awaiter == null && other.awaiter == null)
            {
                return true;
            }

            if (this.awaiter != null && other.awaiter != null)
            {
                return this.awaiter == other.awaiter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (this.awaiter == null)
            {
                return 0;
            }

            return this.awaiter.GetHashCode();
        }

        public override string ToString()
        {
            return this.awaiter == null? "()"
                    : this.awaiter.Status == AwaiterStatus.Succeeded? "()"
                    : "(" + this.awaiter.Status + ")";
        }

        public struct Awaiter: IAwaiter
        {
            private readonly ECSTask task;

            [DebuggerHidden]
            public Awaiter(ECSTask task)
            {
                this.task = task;
            }

            [DebuggerHidden]
            public bool IsCompleted => task.IsCompleted;

            [DebuggerHidden]
            public AwaiterStatus Status => task.Status;

            [DebuggerHidden]
            public void GetResult()
            {
                task.GetResult();
            }

            [DebuggerHidden]
            public void OnCompleted(Action continuation)
            {
                if (task.awaiter != null)
                {
                    task.awaiter.OnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }

            [DebuggerHidden]
            public void UnsafeOnCompleted(Action continuation)
            {
                if (task.awaiter != null)
                {
                    task.awaiter.UnsafeOnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }
        }
    }

    /// <summary>
    /// Lightweight unity specified task-like object.
    /// </summary>
    [AsyncMethodBuilder(typeof (ETAsyncTaskMethodBuilder<>))]
    public struct ECSTask<T>: IEquatable<ECSTask<T>>
    {
        private readonly T result;
        private readonly IAwaiter<T> awaiter;

        [DebuggerHidden]
        public ECSTask(T result)
        {
            this.result = result;
            this.awaiter = null;
        }

        [DebuggerHidden]
        public ECSTask(IAwaiter<T> awaiter)
        {
            this.result = default;
            this.awaiter = awaiter;
        }

        [DebuggerHidden]
        public AwaiterStatus Status => awaiter?.Status ?? AwaiterStatus.Succeeded;

        [DebuggerHidden]
        public bool IsCompleted => awaiter?.IsCompleted ?? true;

        [DebuggerHidden]
        public T Result
        {
            get
            {
                if (awaiter == null)
                {
                    return result;
                }

                return this.awaiter.GetResult();
            }
        }
        
        public void Coroutine()
        {
        }

        [DebuggerHidden]
        public Awaiter GetAwaiter()
        {
            return new Awaiter(this);
        }

        public bool Equals(ECSTask<T> other)
        {
            if (this.awaiter == null && other.awaiter == null)
            {
                return EqualityComparer<T>.Default.Equals(this.result, other.result);
            }

            if (this.awaiter != null && other.awaiter != null)
            {
                return this.awaiter == other.awaiter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (this.awaiter == null)
            {
                if (result == null)
                {
                    return 0;
                }

                return result.GetHashCode();
            }

            return this.awaiter.GetHashCode();
        }

        public override string ToString()
        {
            return this.awaiter == null? result.ToString()
                    : this.awaiter.Status == AwaiterStatus.Succeeded? this.awaiter.GetResult().ToString()
                    : "(" + this.awaiter.Status + ")";
        }

        public static implicit operator ECSTask(ECSTask<T> task)
        {
            if (task.awaiter != null)
            {
                return new ECSTask(task.awaiter);
            }

            return new ECSTask();
        }

        public struct Awaiter: IAwaiter<T>
        {
            private readonly ECSTask<T> task;

            [DebuggerHidden]
            public Awaiter(ECSTask<T> task)
            {
                this.task = task;
            }

            [DebuggerHidden]
            public bool IsCompleted => task.IsCompleted;

            [DebuggerHidden]
            public AwaiterStatus Status => task.Status;

            [DebuggerHidden]
            void IAwaiter.GetResult()
            {
                GetResult();
            }

            [DebuggerHidden]
            public T GetResult()
            {
                return task.Result;
            }

            [DebuggerHidden]
            public void OnCompleted(Action continuation)
            {
                if (task.awaiter != null)
                {
                    task.awaiter.OnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }

            [DebuggerHidden]
            public void UnsafeOnCompleted(Action continuation)
            {
                if (task.awaiter != null)
                {
                    task.awaiter.UnsafeOnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }
        }
    }
}