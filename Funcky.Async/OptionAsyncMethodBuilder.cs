using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Funcky.Monads;
using ThreadingTask = System.Threading.Tasks.Task;

namespace Funcky.Async
{
    public struct OptionAsyncMethodBuilder<TItem>
        where TItem : notnull
    {
        private IAsyncStateMachine _stateMachine;

        public static OptionAsyncMethodBuilder<TItem> Create() => new();

        public void Start<TStateMachine>(ref TStateMachine machine)
            where TStateMachine : IAsyncStateMachine => machine.MoveNext();

        public void SetStateMachine(IAsyncStateMachine machine)
            => _stateMachine = machine;

        public void SetException(Exception exception)
            => Task = OptionAsync.From(new ValueTask<Option<TItem>>(ThreadingTask.FromException<Option<TItem>>(exception)));

        public void SetResult(Option<TItem> result)
            => Task = OptionAsync.From(result);

        public void SetResult(TItem result)
            => Task = OptionAsync.Some(result);

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine machine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            var stateMachine = _stateMachine;
            awaiter.OnCompleted(() => stateMachine.MoveNext());
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine machine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            var stateMachine = _stateMachine;
            awaiter.OnCompleted(() => stateMachine.MoveNext());
        }

        public OptionAsync<TItem> Task { get; private set; }
    }
}
