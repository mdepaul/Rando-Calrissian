using MD.RandoCalrissian;
using Xunit;

namespace UnitTests
{
    public class SortingTests
    {
        [Fact]
        public void Sort()
        {
            const string ordered = "abcdefghi";
            const string unordered = "bcafedihg";

            Assert.Equal(ordered, unordered.Sort());
        }

        [Fact]
        public void Reverse()
        {
            const string ordered = "abcdefghi";
            const string reversed = "ihgfedcba";

            Assert.Equal(reversed, ordered.Reverse());
        }
    }
}
