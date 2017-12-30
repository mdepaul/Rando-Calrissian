/* Attributions
 * Mike DePaul
 * https://github.com/mdepaul/Rando-Calrissian.git
 * **/


namespace MD.RandoCalrissian
{
    public class DiceMaker
    {
        IPrng Prng;
        RandomizedList<int> D4;
        RandomizedList<int> D6;
        RandomizedList<int> D8;
        RandomizedList<int> D10;
        RandomizedList<int> D12;
        RandomizedList<int> D20;

        public DiceMaker(IPrng pseudoRandomNumberGenerator)
        {
            Prng = pseudoRandomNumberGenerator;

            D4 = new RandomizedList<int>(Prng);
            D6 = new RandomizedList<int>(Prng);
            D8 = new RandomizedList<int>(Prng);
            D10 = new RandomizedList<int>(Prng);
            D12 = new RandomizedList<int>(Prng);
            D20 = new RandomizedList<int>(Prng);

            Fill();
        }


        void Fill()
        {
            Fill(D4, 4);
            Fill(D6, 6);
            Fill(D8, 8);
            Fill(D10, 10);
            Fill(D12, 12);
            Fill(D20, 20);
        }
        void Fill(RandomizedList<int> die, int count)
        {
            die.Clear();
            for (int i = 1; i <= count; i++)
            {
                die.Add(i);
            }
        }

        public int Roll(int numberOfD4 = 0, int numberOfD6 = 0, int numberOfD8 = 0, int numberOfD10 = 0, int numberOfD12 = 0, int numberOfD20 = 0)
        {
            int total = 0;

            total += Roll(D4, numberOfD4);
            total += Roll(D6, numberOfD6);
            total += Roll(D8, numberOfD8);
            total += Roll(D10, numberOfD10);
            total += Roll(D12, numberOfD12);
            total += Roll(D20, numberOfD20);

            return total;
        }

        int Roll(RandomizedList<int> die, int numberOfTimes)
        {
            int total = 0;
            if (numberOfTimes < 1)
                return total;

            for (int i = 0; i < numberOfTimes; i++)
            {
                //Each roll, we need to get re-seed the die, and get whatever is on top.
                Fill(die, die.Count);
                total += die[0];
            }

            return total;
        }
    }
}
