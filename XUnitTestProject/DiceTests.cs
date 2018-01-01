using MD.RandoCalrissian;
using System.Collections.Generic;
using Xunit;

namespace UnitTests
{

    public class DiceTests
    {
        [Fact]
        public void ConfirmDice()
        {
            var results = GetDiceResults(4);
            foreach (var item in results)
                Assert.True(item > 0 && item < 5);

            results = GetDiceResults(6);
            foreach (var item in results)
                Assert.True(item > 0 && item < 7);

            results = GetDiceResults(8);
            foreach (var item in results)
                Assert.True(item > 0 && item < 9);

            results = GetDiceResults(10);
            foreach (var item in results)
                Assert.True(item > 0 && item < 11);

            results = GetDiceResults(12);
            foreach (var item in results)
                Assert.True(item > 0 && item < 13);

            results = GetDiceResults(20);
            foreach (var item in results)
                Assert.True(item > 0 && item < 21);
        }

        /// <summary>
        /// Performs 1000 dice rolls and returns the result of each roll.
        /// </summary>
        /// <param name="dice"></param>
        /// <returns></returns>
        public List<int> GetDiceResults(int dice)
        {
            const int rolls = 1000;
            List<int> results = new List<int>();
            for (int i = 0; i < rolls; i++)
            {
                results.Add(new Crypto(new PrngSHA256()).RollDice(dice == 4 ? 1 : 0, dice == 6 ? 1 : 0, dice == 8 ? 1 : 0, dice == 10 ? 1 : 0, dice == 12 ? 1 : 0, dice == 20 ? 1 : 0));
            }

            return results;
        }
    }
}
