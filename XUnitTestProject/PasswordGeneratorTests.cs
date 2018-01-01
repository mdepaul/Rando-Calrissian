using MD.RandoCalrissian;
using System;
using Xunit;

namespace UnitTests
{

    public class PasswordGeneratorTests
    {

        [Fact]
        public void TestGetRandomCharactersLength()
        {
            const string characterSpace = "abcdefghijklmnopqrstuvwzyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789~!@#$%^&*()";
            string randomized = new Crypto(new PrngSHA256()).GetRandomCharacters(characterSpace.Length, characterSpace);

            Assert.True(characterSpace.Length == randomized.Length);
        }

        [Fact]
        public void TestGetRandomCharactersNotEqual()
        {
            const string characterSpace = "abcdefghijklmnopqrstuvwzyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789~!@#$%^&*()";
            string randomized = new Crypto(new PrngSHA256()).GetRandomCharacters(characterSpace.Length, characterSpace);

            Assert.NotEqual(characterSpace, randomized);
        }

        [Fact]
        public void MakePassword()
        {
            string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string Lower = "abcdefghijklmnopqrstuvwzyz";
            string Digits = "23456789";  //0 and 1 removed to prevent ambiguity
            string Special = "~!@#$%^&* ()_+.";
            int totalLength = 32;
            int upperRequired = 2;
            int lowerRequired = 2;
            int digitsRequired = 2;
            int specialsRequired = 2;
            CharacterMix mix = new CharacterMix();

            string password = new Crypto(new PrngSHA256()).MakePassword(totalLength, mix, Upper, upperRequired, Lower, lowerRequired, Digits, digitsRequired, Special, specialsRequired);

            int upperCount = 0;
            int lowerCount = 0;
            int digitCount = 0;
            int specialCount = 0;
            for (int i = 0; i < password.Length; i++)
            {
                if (Upper.Contains(password.Substring(i, 1)))
                    upperCount++;

                if (Lower.Contains(password.Substring(i, 1)))
                    lowerCount++;

                if (Digits.Contains(password.Substring(i, 1)))
                    digitCount++;

                if (Special.Contains(password.Substring(i, 1)))
                    specialCount++;
            }

            Assert.Equal(password.Length, totalLength);
            Assert.True(upperCount >= upperRequired);
            Assert.True(lowerCount >= lowerRequired);
            Assert.True(digitCount >= digitsRequired);
            Assert.True(specialCount >= specialsRequired);

        }

        [Theory]
        [InlineData(8, 3, 2, 2, 2)]
        [InlineData(8, 2, 3, 2, 2)]
        [InlineData(8, 2, 2, 3, 2)]
        [InlineData(8, 2, 2, 2, 3)]
        [InlineData(7, 2, 2, 2, 2)]
        public void InvalidMinimumRequiredParameter(int totalLength, int upper, int lower, int digits, int special)
        {
            string password;

            Exception ex = Assert.Throws<ArgumentException>(() =>
    password = new Crypto(new PrngSHA256()).MakePassword(totalLength, upper, lower, digits, special)
);
        }


        [Theory]
        [InlineData("", "abcdefghijklmnopqrstuvwzyz", "23456789", "~!@#$%^&*()_+.")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "", "23456789", "~!@#$%^&*()_+.")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwzyz", "", "~!@#$%^&*()_+.")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwzyz", "23456789", "")]
        public void InvalidCharacterSpace(string upper, string lower, string digits, string special)
        {
            string Upper = upper;
            string Lower = lower;
            string Digits = digits;  //0 and 1 removed to prevent ambiguity
            string Special = special;
            int totalLength = 32;
            int upperRequired = 2;
            int lowerRequired = 2;
            int digitsRequired = 2;
            int specialsRequired = 2;
            CharacterMix mix = new CharacterMix();

            string password;

            Exception ex = Assert.Throws<ArgumentException>(() =>
                password = new Crypto(new PrngSHA256()).MakePassword(totalLength, mix, Upper, upperRequired, Lower, lowerRequired, Digits, digitsRequired, Special, specialsRequired)
            );
        }

        [Theory]
        [InlineData(false, true, true, true)]
        [InlineData(true, false, true, true)]
        [InlineData(true, true, false, true)]
        [InlineData(true, true, true, false)]
        public void InvalidCharacterMix(bool useUpper, bool useLower, bool useDigits, bool useSpecial)
        {
            string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string Lower = "abcdefghijklmnopqrstuvwzyz";
            string Digits = "23456789";  //0 and 1 removed to prevent ambiguity
            string Special = "~!@#$%^&*()_+.";
            int totalLength = 32;
            int upperRequired = 2;
            int lowerRequired = 2;
            int digitsRequired = 2;
            int specialsRequired = 2;

            string invalidCharacters = "";
            if (useUpper == false)
            {
                invalidCharacters = Upper;
            }
            else if (useLower == false)
            {
                invalidCharacters = Lower;
            }
            else if (useDigits == false)
            {
                invalidCharacters = Digits;
            }
            else if (useSpecial == false)
            {
                invalidCharacters = Special;
            }


            CharacterMix mix = new CharacterMix()
            {
                UseUpper = useUpper,
                UseLower = useLower,
                UseDigits = useDigits,
                UseSpecial = useSpecial
            };

            string password = new Crypto(new PrngSHA256()).MakePassword(totalLength, mix, Upper, upperRequired, Lower, lowerRequired, Digits, digitsRequired, Special, specialsRequired);

            for (int i = 0; i < password.Length; i++)
            {
                Assert.True(
                    !invalidCharacters.Contains(password.Substring(i, 1))
                    );
            }
        }

    }
}
