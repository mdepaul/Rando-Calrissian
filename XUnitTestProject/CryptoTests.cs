using MD.RandoCalrissian;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTests
{

    class Stats
    {
        public double Count { get; set; }
        public double Percent { get; set; }
    }

    public class CryptoTests
    {
        [Fact]
        public void TestGetCharacterStats()
        {
            IPrng prng = new PrngSHA256();
            Crypto myCrypto = new Crypto(prng);
            const double numberOfRandomCharactersDesired = 1000000;
            string characterSpace = "abcdefghijklmnopqrstuvwxyz";

            Dictionary<char, Stats> characterMap = new Dictionary<char, Stats>();
            for (int i = 0; i < characterSpace.Length; i++)
            {
                characterMap.Add(characterSpace.ToCharArray()[i], new Stats());
            }

            char[] randomCharacters = myCrypto.GetRandomCharacters((int)numberOfRandomCharactersDesired, characterSpace).ToCharArray();

            for (int i = 0; i < randomCharacters.Length; i++)
            {
                characterMap[randomCharacters[i]].Count++;
            }

            const double margin = 1.5;  //Each character must must be within 1.5% of the expected perfect distribution (98.5%-101.5%)
            double expected = numberOfRandomCharactersDesired / ((double)characterSpace.Length);
            double actual;
            foreach (var mix in characterMap)
            {
                actual = ((mix.Value.Count / expected) * 100);
                mix.Value.Percent = actual;
                string message = String.Format("Bad Character {0} @ {1}", mix.Key.ToString(), mix.Value.Percent);
                Assert.True(actual < 100 + margin, message);
                Assert.True(actual > 100 - margin, message);
            }
        }

        [Fact]
        public void GetRandomItems()
        {
            string path = "pw.txt";
            int count = 5;
            List<string> result = new Crypto(new PrngSHA256()).GetRandomItems(path, count);
            Assert.True(result.Count == count);
        }

        //http://hadihariri.com/2008/10/17/testing-exceptions-with-xunit/
        [Fact]
        public void GetRandomItemsBadFile()
        {
            string path = "pw_x.txt";
            int count = 5;
            List<string> result;

            Exception ex = Assert.Throws<FileNotFoundException>(() =>
                result = new Crypto(new PrngSHA256()).GetRandomItems(path, count)
            );
        }

        [Fact]
        public void GetRandomItemsTooManyRequested()
        {
            string path = "pw.txt";
            int countDesired = 65;
            List<string> result;

            Exception ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
                result = new Crypto(new PrngSHA256()).GetRandomItems(path, countDesired)
            );
        }

        [Fact]
        public void GetRandomItemsTooManyRequestedOverride()
        {
            string path = "pw.txt";
            int countDesired = 65;
            int countAvailable = 64;
            List<string> result = new Crypto(new PrngSHA256()).GetRandomItems(path, countDesired, true);
            Assert.True(result.Count == countAvailable);
        }
    }
}
