/* Attributions
 * Mike DePaul
 * https://github.com/mdepaul/Rando-Calrissian.git
 * **/

using System;
using System.Text;

namespace MD.RandoCalrissian
{
    public class PasswordMaker
    {
        IPrng Prng;
        const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string Lower = "abcdefghijklmnopqrstuvwzyz";
        const string Digits = "23456789";  //0 and 1 ommitted to prevent ambiguity with O, i and L
        const string Special = "~!@#$%^&*()_+.";

        private PasswordMaker()
        {

        }

        public PasswordMaker(IPrng prng)
        {
            Prng = prng;
        }
        public string Make(int totalLength, int numberUpperCase, int numberLowerCase, int numberDigit, int numberSpecial)
        {
            return Make(totalLength, new CharacterMix(), Upper, numberUpperCase, Lower, numberLowerCase, Digits, numberDigit, Special, numberSpecial);
        }

        public string Make(int totalLength, CharacterMix mix, int numberUpperCase, int numberLowerCase, int numberDigit, int numberSpecial)
        {
            return Make(totalLength, mix, Upper, numberUpperCase, Lower, numberLowerCase, Digits, numberDigit, Special, numberSpecial);
        }

        public string Make(int totalLength, CharacterMix mix, string upperCaseChars, int numberUpperCase, string lowerCaseChars, int numberLowerCase, string digitChars, int numberDigit, string specialChars, int numberSpecial)
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
    }
}
