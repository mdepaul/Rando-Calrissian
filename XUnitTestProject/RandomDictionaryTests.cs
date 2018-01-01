using MD.RandoCalrissian;
using System.Collections.Generic;
using Xunit;

namespace UnitTests
{
    public class RandomDictionaryTests
    {
        [Fact]
        public void RandomListIsRandom()
        {
            RandomizedList<string> randoList = new RandomizedList<string>(new PrngSHA256());
            List<string> baseList = new List<string>();

            baseList.Add("a");
            baseList.Add("A");
            baseList.Add("b");
            baseList.Add("B");
            baseList.Add("CCC");
            baseList.Add("D");
            baseList.Add("0");
            baseList.Add("1");
            baseList.Add("$^%#$%^%$#");

            foreach (var item in baseList)
            {
                randoList.Add(item);
            }

            bool allMatch = true;
            for (int i = 0; i < baseList.Count; i++)
            {
                if (baseList[i] != randoList[i])
                    allMatch = false;
            }

            Assert.False(allMatch);
        }

        [Fact]
        public void RandomListIsCorrectSize()
        {
            RandomizedList<string> randoList = new RandomizedList<string>(new PrngSHA256());
            List<string> baseList = new List<string>();

            baseList.Add("a");
            baseList.Add("A");
            baseList.Add("b");
            baseList.Add("B");
            baseList.Add("CCC");
            baseList.Add("D");
            baseList.Add("0");
            baseList.Add("1");
            baseList.Add("$^%#$%^%$#");

            foreach (var item in baseList)
            {
                randoList.Add(item);
            }

            Assert.True(randoList.Count == baseList.Count);
        }
    }
}
