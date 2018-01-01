using MD.RandoCalrissian;
using System;
using Xunit;

namespace UnitTests
{
    public class PrngSha256Tests
    {

        [Fact]
        public void GetRandomBytes()
        {
            int numberOfBytes = 32;
            byte[] bytes = new PrngSHA256().GetRandomBytes(numberOfBytes);

            Assert.True(bytes.Length == numberOfBytes);
        }

        [Fact]
        public void GetRandomBytesExtras()
        {
            int numberOfBytes = 64;
            byte[] bytes = new PrngSHA256().GetRandomBytes(numberOfBytes);

            Assert.True(bytes.Length == numberOfBytes);
        }

        [Fact]
        public void InvalidBytes()
        {
            byte[] bytes;
            Exception ex = Assert.Throws<ArgumentException>(() =>
                bytes = new PrngSHA256().GetRandomBytes(-1)
            );

        }
    }
}
