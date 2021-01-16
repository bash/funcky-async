using System;
using System.Threading.Tasks;
using Funcky.Monads;
using static Funcky.Functional;

namespace Funcky.Async
{
    public static class OptionExtensions
    {
        public static async Task<Option<TResult>> SelectAsync<TItem, TResult>(
            this Option<TItem> option,
            Func<TItem, Task<TResult>> selector)
            where TItem : notnull
            where TResult : notnull
            => await option.Select(selector);

        public static async Task<Option<TResult>> SelectManyAsync<TItem, TResult>(
            this Option<TItem> option,
            Func<TItem, Task<Option<TResult>>> selector)
            where TItem : notnull
            where TResult : notnull
            => (await option.Select(selector)).SelectMany(Identity);

        // TODO: SelectMany with two selectors

        public static async Task<Option<TItem>> WhereAsync<TItem>(this Option<TItem> option, Func<TItem, Task<bool>> predicate)
            where TItem : notnull
            => await option.SelectManyAsync(async item => await predicate(item) ? item : Option<TItem>.None());

        public static async Task<Option<TItem>> OrElseAsync<TItem>(this Option<TItem> option, Func<Task<Option<TItem>>> elseOption)
            where TItem : notnull
            => await option.Match(none: elseOption, some: item => Task.FromResult(Option.Some(item)));

        public static async Task<TItem> GetOrElseAsync<TItem>(this Option<TItem> option, Func<Task<TItem>> elseOption)
            where TItem : notnull
            => await option.Match(none: elseOption, some: Task.FromResult);

        public static async Task<Option<TResult>> AndThenAsync<TItem, TResult>(this Option<TItem> option, Func<TItem, Task<TResult>> andThenFunction)
            where TItem : notnull
            where TResult : notnull
            => await option.SelectAsync(andThenFunction);

        public static async Task<Option<TResult>> AndThenAsync<TItem, TResult>(this Option<TItem> option, Func<TItem, Task<Option<TResult>>> andThenFunction)
            where TItem : notnull
            where TResult : notnull
            => await option.SelectManyAsync(andThenFunction);

        public static async Task<Option<TItem>> InspectAsync<TItem>(this Option<TItem> option, Func<TItem, Task> action)
            where TItem : notnull
        {
            await option.AndThen(action);
            return option;
        }
    }
}
