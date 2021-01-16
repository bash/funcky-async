using System.Threading.Tasks;
using Funcky.Monads;
using Xunit;

namespace Funcky.Async.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var result = await Option.Some(10)
                .SelectManyAsync(SquareEvenAsync)
                .Select(SubtractFive)
                .GetOrElse(10);
            Assert.Equal(95, result);
        }

        private static Task<Option<int>> SquareEvenAsync(int n)
            => Task.FromResult(Option.Some(n * n).Where(n => n % 2 == 0));

        private static int SubtractFive(int n) => n - 5;
    }
}
