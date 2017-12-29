using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/**
 * TODO: This can be Crypto, I suppose.
 * The GitHub project will be Rando Calrissian.
 * The C# project will be Rando.
 * 
 * Options:
 *  Make Password (u,l,d,s)
 *  Make Key: Kex, Base64, Base64Safe
 *  Roll Die: d4, d6, d10, d12, d20
 *  Pick item: Pick from CrLf list in a text file
 *      -Provide starter lists?
 *      -Names
 *      -TV Shows
 *      -Dinner meals
 *      -Dates
 *      -?
 * */

namespace MD.RandoCalrissian
{
    public class Crypto
    {
        IPrng Prng;
        const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string Lower = "abcdefghijklmnopqrstuvwzyz";
        const string Digits = "23456789";  //0 and 1 ommitted to prevent ambiguity with O, i and L
        const string Special = "~!@#$%^&*()_+.";

        RandomizedList<int> D4;
        RandomizedList<int> D6;
        RandomizedList<int> D8;
        RandomizedList<int> D10;
        RandomizedList<int> D12;
        RandomizedList<int> D20;

        /// <summary>
        /// Creates a new Crypto object for performing some handy functions.
        /// <para>
        /// Mostly, this takes pseudo-random bytes and encodes them in a variety of useful ways for:
        /// <para>Password generation</para>
        /// <para>Dice rolling</para>
        /// <para>Key generation (hex & base64)</para>
        /// <para>Selecting random items from a CrLf-delimited file</para>
        /// </para>
        /// </summary>
        /// <param name="pseudoRandomNumberGenerator">A Pseudo-random number generator that implements IPrng</param>
        public Crypto(IPrng pseudoRandomNumberGenerator)
        {
            Prng = pseudoRandomNumberGenerator;

            D4 = new RandomizedList<int>(Prng);
            D6 = new RandomizedList<int>(Prng);
            D8 = new RandomizedList<int>(Prng);
            D10 = new RandomizedList<int>(Prng);
            D12 = new RandomizedList<int>(Prng);
            D20 = new RandomizedList<int>(Prng);

            FillDie();
        }


        void FillDie()
        {
            FillDie(D4, 4);
            FillDie(D6, 6);
            FillDie(D8, 8);
            FillDie(D10, 10);
            FillDie(D12, 12);
            FillDie(D20, 20);
        }
        void FillDie(RandomizedList<int> die, int count)
        {
            die.Clear();
            for (int i = 1; i <= count; i++)
            {
                die.Add(i);
            }
        }

        public char GetCharacter(byte randomByte, string characterSpace)
        {
            int position = XConvert.ToInt32(randomByte);
            position = position % characterSpace.Length;
            return characterSpace.ToCharArray()[position];
        }

        public string GetRandomCharacters(int number, string characterSpace)
        {
            StringBuilder sb = new StringBuilder(number);
            byte[] tempBytes = Prng.GetRandomBytes(number);

            for (int i = 0; i < number; i++)
            {
                //Re-order the character space so that there are no characters with less probability from the modular division.
                //Re-shuffle after pulling the number of characters equal to its length
                if (i % characterSpace.Length == 0)
                    characterSpace = ShuffleString(new StringBuilder(characterSpace));

                sb.Append(GetCharacter(tempBytes[i], characterSpace));
            }
            return sb.ToString();
        }

        public string MakePassword(int totalLength, int numberUpperCase, int numberLowerCase, int numberDigit, int numberSpecial)
        {
            return MakePassword(totalLength, new CharacterMix(), Upper, numberUpperCase, Lower, numberLowerCase, Digits, numberDigit, Special, numberSpecial);
        }

        public string MakePassword(int totalLength, CharacterMix mix, int numberUpperCase, int numberLowerCase, int numberDigit, int numberSpecial)
        {
            return MakePassword(totalLength, mix, Upper, numberUpperCase, Lower, numberLowerCase, Digits, numberDigit, Special, numberSpecial);
        }

        public string MakePassword(int totalLength, CharacterMix mix, string upperCaseChars, int numberUpperCase, string lowerCaseChars, int numberLowerCase, string digitChars, int numberDigit, string specialChars, int numberSpecial)
        {

            if ((numberUpperCase + numberLowerCase + numberDigit + numberSpecial) > totalLength)
            {
                throw new ArgumentException(String.Format("Required {0}, but only allowing for {1}", numberUpperCase + numberLowerCase + numberDigit + numberSpecial, totalLength));
            }

            if (numberUpperCase > 0 && upperCaseChars.Length == 0)
            {
                throw new ArgumentException(String.Format("Required {0} {1} characters, but didn't provide a character list.", numberUpperCase, "Upper"));
            }

            if (numberLowerCase > 0 && lowerCaseChars.Length == 0)
            {
                throw new ArgumentException(String.Format("Required {0} {1} characters, but didn't provide a character list.", numberLowerCase, "Digit"));
            }

            if (numberDigit > 0 && digitChars.Length == 0)
            {
                throw new ArgumentException(String.Format("Required {0} {1} characters, but didn't provide a character list.", numberDigit, "Special"));
            }

            if (numberSpecial > 0 && specialChars.Length == 0)
            {
                throw new ArgumentException(String.Format("Required {0} {1} characters, but didn't provide a character list.", numberSpecial, "Upper"));
            }

            StringBuilder sb = new StringBuilder();
            if (mix.UseUpper)
                sb.Append(GetRandomCharacters(numberUpperCase, upperCaseChars));

            if (mix.UseLower)
                sb.Append(GetRandomCharacters(numberLowerCase, lowerCaseChars));

            if (mix.UseDigits)
                sb.Append(GetRandomCharacters(numberDigit, digitChars));

            if (mix.UseSpecial)
                sb.Append(GetRandomCharacters(numberSpecial, specialChars));

            if (sb.Length < totalLength)
            {
                StringBuilder charactersAvailable = new StringBuilder();
                charactersAvailable.Append(mix.UseUpper ? upperCaseChars : "");
                charactersAvailable.Append(mix.UseLower ? lowerCaseChars : "");
                charactersAvailable.Append(mix.UseDigits ? digitChars : "");
                charactersAvailable.Append(mix.UseSpecial ? specialChars : "");

                sb.Append(GetRandomCharacters(totalLength - sb.Length, charactersAvailable.ToString()));
            }

            return ShuffleString(sb);
        }

        /// <summary>
        /// Re-arranges the characters in the stringbuilder using the entropy from a strong pseudo-random number generator.
        /// </summary>
        /// <param name="sb">The StringbBuilder input</param>
        /// <returns>A string with all of the characters from the input StringBuilder, but with the characters randomly spread</returns>
        public string ShuffleString(StringBuilder sb)
        {
            RandomizedList<char> rando = new RandomizedList<char>(Prng);
            for (int i = 0; i < sb.Length; i++)
            {
                rando.Add(sb[i]);
            }

            sb.Clear();
            foreach (var item in rando)
            {
                sb.Append(item);
            }
            return sb.ToString();
        }

        public string GenerateRandomHexadecimal(int numberOfBytes)
        {
            return Prng.GetRandomBytes(numberOfBytes).ToHex();
        }

        public string GenerateRandomBase64String(int numberOfBytes)
        {
            return Prng.GetRandomBytes(numberOfBytes).ToBase64String();
        }

        public string GenerateRandomSafeBase64String(int numberOfBytes)
        {
            return Prng.GetRandomBytes(numberOfBytes).ToSafeBase64String();
        }

        public List<string> GetRandomItems(string filepath, int numberToReturn, bool returnWhatsAvailable = false)
        {
            RandomizedList<string> rando = new RandomizedList<string>(Prng);
            if (!System.IO.File.Exists(filepath))
                throw new FileNotFoundException(String.Format("\"{0}\" was not found.", filepath));

            string[] lines = System.IO.File.ReadAllLines(filepath);
            foreach (var line in lines)
            {
                if (!String.IsNullOrEmpty(line))
                    rando.Add(line);
            }

            if (!returnWhatsAvailable && rando.Count < numberToReturn)
                throw new ArgumentOutOfRangeException(String.Format("Requested {0} items, but only {1} are available", numberToReturn, rando.Count));

            List<string> result = new List<string>();
            int addIndex = 0;
            while (result.Count < numberToReturn && addIndex < rando.Count)
            {
                result.Add(rando[addIndex]);
                addIndex++;
            }

            return result;
        }

        public int RollDice(int numberOfD4 = 0, int numberOfD6 = 0, int numberOfD8 = 0, int numberOfD10 = 0, int numberOfD12 = 0, int numberOfD20 = 0)
        {
            int total = 0;

            total += RollDie(D4, numberOfD4);
            total += RollDie(D6, numberOfD6);
            total += RollDie(D8, numberOfD8);
            total += RollDie(D10, numberOfD10);
            total += RollDie(D12, numberOfD12);
            total += RollDie(D20, numberOfD20);

            return total;
        }

        int RollDie(RandomizedList<int> die, int numberOfTimes)
        {
            int total = 0;
            if (numberOfTimes < 1)
                return total;

            for (int i = 0; i < numberOfTimes; i++)
            {
                //Each roll, we need to get re-seed the die, and get whatever is on top.
                FillDie(die, die.Count);
                total += die[0];
            }

            return total;
        }
    }
}
