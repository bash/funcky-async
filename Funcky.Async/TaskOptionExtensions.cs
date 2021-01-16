using System;
using System.Threading.Tasks;
using Funcky.Monads;
using static Funcky.Functional;

namespace Funcky.Async
{
    public static class TaskOptionExtensions
    {
        public static async Task<Option<TResult>> Select<TItem, TResult>(
            this Task<Option<TItem>> option,
            Func<TItem, TResult> selector)
            where TItem : notnull
            where TResult : notnull
            => (await option).Select(selector);

        public static async Task<Option<TResult>> SelectAsync<TItem, TResult>(
            this Task<Option<TItem>> option,
            Func<TItem, Task<TResult>> selector)
            where TItem : notnull
            where TResult : notnull
            => await (await option).Select(selector);

        public static async Task<Option<TResult>> SelectMany<TItem, TResult>(
            this Task<Option<TItem>> option,
            Func<TItem, Option<TResult>> selector)
            where TItem : notnull
            where TResult : notnull
            => (await option).SelectMany(selector);

        public static async Task<Option<TResult>> SelectManyAsync<TItem, TResult>(
            this Task<Option<TItem>> option,
            Func<TItem, Task<Option<TResult>>> selector)
            where TItem : notnull
            where TResult : notnull
            => (await (await option).Select(selector)).SelectMany(Identity);

        public static async Task<Option<TItem>> Where<TItem>(this Task<Option<TItem>> option, Func<TItem, bool> predicate)
            where TItem : notnull
            => (await option).Where(predicate);

        public static async Task<Option<TItem>> WhereAsync<TItem>(this Task<Option<TItem>> option, Func<TItem, Task<bool>> predicate)
            where TItem : notnull
            => await (await option).WhereAsync(predicate);

        public static async Task<Option<TItem>> OrElse<TItem>(this Task<Option<TItem>> option, Func<Option<TItem>> elseOption)
            where TItem : notnull
            => (await option).OrElse(elseOption);

        public static async Task<Option<TItem>> OrElseAsync<TItem>(this Option<TItem> option, Func<Task<Option<TItem>>> elseOption)
            where TItem : notnull
            => await option.Match(none: elseOption, some: item => Task.FromResult(Option.Some(item)));

        public static async Task<TItem> GetOrElse<TItem>(this Task<Option<TItem>> option, TItem elseOption)
            where TItem : notnull
            => (await option).GetOrElse(elseOption);

        public static async Task<TItem> GetOrElse<TItem>(this Task<Option<TItem>> option, Func<TItem> elseOption)
            where TItem : notnull
            => (await option).GetOrElse(elseOption);

        public static async Task<TItem> GetOrElseAsync<TItem>(this Task<Option<TItem>> option, Func<Task<TItem>> elseOption)
            where TItem : notnull
            => await (await option).GetOrElseAsync(elseOption);

        public static async Task<Option<TResult>> AndThen<TItem, TResult>(this Task<Option<TItem>> option, Func<TItem, TResult> andThenFunction)
            where TItem : notnull
            where TResult : notnull
            => (await option).Select(andThenFunction);

        public static async Task<Option<TResult>> AndThenAsync<TItem, TResult>(this Task<Option<TItem>> option, Func<TItem, Task<TResult>> andThenFunction)
            where TItem : notnull
            where TResult : notnull
            => await option.SelectAsync(andThenFunction);

        public static async Task<Option<TResult>> AndThen<TItem, TResult>(this Task<Option<TItem>> option, Func<TItem, Option<TResult>> andThenFunction)
            where TItem : notnull
            where TResult : notnull
            => (await option).SelectMany(andThenFunction);

        public static async Task<Option<TResult>> AndThenAsync<TItem, TResult>(this Task<Option<TItem>> option, Func<TItem, Task<Option<TResult>>> andThenFunction)
            where TItem : notnull
            where TResult : notnull
            => await (await option).SelectManyAsync(andThenFunction);

        public static async Task<Option<TItem>> Inspect<TItem>(this Task<Option<TItem>> option, Action<TItem> action)
            where TItem : notnull
            => (await option).Inspect(action);

        public static async Task<Option<TItem>> InspectAsync<TItem>(this Task<Option<TItem>> option, Func<TItem, Task> action)
            where TItem : notnull
            => await (await option).InspectAsync(action);
    }
}
