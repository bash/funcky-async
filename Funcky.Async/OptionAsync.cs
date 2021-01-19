using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Funcky.Monads;

namespace Funcky.Async
{
    [AsyncMethodBuilder(typeof(OptionAsyncMethodBuilder<>))]
    public readonly struct OptionAsync<TItem>
        where TItem : notnull
    {
        private readonly ValueTask<Option<TItem>> _item;

        internal OptionAsync(ValueTask<Option<TItem>> item) => _item = item;

        public async OptionAsync<TResult> Select<TResult>(Func<TItem, TResult> selector)
            where TResult : notnull
        {
            return (await this).Select(selector);
        }

        public static OptionAsync<TItem> None() => default;

        public OptionAsyncAwaiter<TItem> GetAwaiter() => new(_item);
    }

    public static class OptionAsync
    {
        public static OptionAsync<TItem> Some<TItem>(TItem item)
            where TItem : notnull
            => new(new ValueTask<Option<TItem>>(Option.Some(item)));

        public static OptionAsync<TItem> From<TItem>(ValueTask<Option<TItem>> valueTask)
            where TItem : notnull
            => new(valueTask);

        public static OptionAsync<TItem> From<TItem>(Option<TItem> valueTask)
            where TItem : notnull
            => new(new ValueTask<Option<TItem>>(valueTask));

        internal static OptionAsync<TItem> From<TItem>(Func<ValueTask<Option<TItem>>> valueTaskFunc)
            where TItem : notnull
            => new(valueTaskFunc());
    }
}
