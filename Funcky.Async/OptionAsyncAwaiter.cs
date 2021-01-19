using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Funcky.Monads;

namespace Funcky.Async
{
    public readonly struct OptionAsyncAwaiter<TItem> : INotifyCompletion
        where TItem : notnull
    {
        private readonly ValueTaskAwaiter<Option<TItem>> _awaiter;

        internal OptionAsyncAwaiter(ValueTask<Option<TItem>> option) => _awaiter = option.GetAwaiter();

        public bool IsCompleted => _awaiter.IsCompleted;

        public void OnCompleted(Action continuation) => _awaiter.OnCompleted(continuation);

        public Option<TItem> GetResult() => _awaiter.GetResult();
    }
}
