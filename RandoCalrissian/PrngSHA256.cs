/* Attribution
 * Mike DePaul
 * https://github.com/mdepaul/XRng.git
 * */
using System.Security.Cryptography;

namespace MD.RandoCalrissian
{

    /// <summary>
    /// A class for generating pseudo-random numbers using RNGCryptoServiceProvider.
    /// Key stretching is used by leveraging and HMACSHA256 internally to generate more bytes based off the initial entropy from the OS.
    /// Using HMACSHA256 means we don't have to poll the OS over and over for more bytes.
    /// </summary>
    public sealed class PrngSHA256 : IPrng
    {
        byte[] RandomBytes;
        byte[] HmacKey;
        int NextByte = 0;
        const int NumberOfRandomBytes = 32;
        const int NumberOfKeyBytes = 16;
        private const string ArgumentExceptionFormat = "{0} is invalid. A non-negative integer is required.";

        /// <summary>
        /// A class for generating pseudo-random numbers using RNGCryptoServiceProvider.
        /// <para>
        /// Key stretching is used by leveraging and HMACSHA256 internally to generate more bytes based off the initial entropy from the OS.
        /// </para>
        /// <para>
        /// Using HMACSHA256 means we don't have to poll the OS over and over for more bytes.
        /// </para>
        /// </summary>
        public PrngSHA256()
        {
            using (RNGCryptoServiceProvider CryptoProvider = new RNGCryptoServiceProvider())
            {
                RandomBytes = new byte[NumberOfRandomBytes];
                CryptoProvider.GetBytes(RandomBytes);

                HmacKey = new byte[NumberOfKeyBytes];
                CryptoProvider.GetBytes(HmacKey);
            }
        }

        /// <summary>
        /// Gets a single random byte
        /// </summary>
        /// <returns></returns>
        public byte GetRandomByte()
        {
            return GetNextByte();
        }

        /// <summary>
        /// Get an array of random bytes
        /// </summary>
        /// <param name="numberOfBytes">The number of bytes desired.</param>
        /// <returns></returns>
        public byte[] GetRandomBytes(int numberOfBytes)
        {
            if (numberOfBytes < 0)
                throw new System.ArgumentException(string.Format(ArgumentExceptionFormat, numberOfBytes));

            byte[] bytes = new byte[numberOfBytes];
            int fillByte = 0;
            while (fillByte < numberOfBytes)
            {
                bytes[fillByte] = GetNextByte();
                fillByte++;
            }
            return bytes;
        }

        byte GetNextByte()
        {
            int thisByte = NextByte % RandomBytes.Length;
            if (NextByte > 0 && thisByte == 0)
            {
                //Reseed whenever we've used up all of the bytes available
                Reseed();
            }
            NextByte++;
            return RandomBytes[thisByte];
        }

        /// <summary>
        /// Re-initializes the bytes in the RandomBytes array.
        /// </summary>
        public void Reseed()
        {
            using (HMACSHA256 Hmac = new HMACSHA256(HmacKey))
            {
                RandomBytes = Hmac.ComputeHash(RandomBytes);
            }
        }
    }
}
