using MD.RandoCalrissian;
using System;
using Xunit;

namespace UnitTests
{
    public class ParserTests
    {

        [Fact]
        public void ParseClipboard()
        {
            string[] args = new string[5];
            args[0] = "e=u";
            args[1] = "b=32";
            args[2] = "u=5";
            args[3] = "C";

            CommandLinePreferences clp = new CommandLinePreferences(args);

            Assert.True(clp.Clipboard);
        }

        [Fact]
        public void ParserVerbose()
        {
            string[] args = new string[5];
            args[0] = "e=u";
            args[1] = "b=32";
            args[2] = "u=5";
            args[3] = "V";

            CommandLinePreferences clp = new CommandLinePreferences(args);

            Assert.True(clp.Verbose);
        }

        [Theory]
        [InlineData(new object[] { new string[] { "e=u", "b=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=l", "b=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=d", "b=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=s", "b=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=ulds", "b=32", "u=8", "l=8", "s=8", "d=8" } })]
        [InlineData(new object[] { new string[] { "e=ulds", "b=32", "u=5", "d=2" } })]
        public void ParserValidPasswordEncoding(string[] args)
        {
            CommandLinePreferences clp = new CommandLinePreferences(args as string[]);
            bool isValid = clp.IsValid();
            Assert.True(clp.ByteEncoding == XEncoding.Password && isValid);
        }


        [InlineData(new object[] { new string[] { "e=", "b=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=ulds", "b=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=ulds", "b=32", "u=8", "d=8", "s=8", "d=9" } })]
        public void ParserInvalidPasswordEncoding(string[] args)
        {
            CommandLinePreferences clp = new CommandLinePreferences(args as string[]);
            Assert.True(clp.ByteEncoding == XEncoding.Password && clp.IsValid());
        }

        [Theory]
        [InlineData(new object[] { new string[] { "e=us", "b=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=ulds", "byte=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=ulds", "bytes=32", "u=5", "d=2" } })]
        public void ParserByteCount(string[] args)
        {
            CommandLinePreferences clp = new CommandLinePreferences(args);

            Assert.True(clp.Bytes == 32);

        }

        [Theory]
        [InlineData((new object[] { new string[] { "e=u", "b=10", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=ul", "b=10", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=uld", "b=10", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=ulds", "b=10", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=dsul", "b=10", "s=1", "u=5", "l=4", "d=2", } }))]
        [InlineData((new object[] { new string[] { "e=dslu", "b=10", "d=2", "s=1", "u=5", "l=4", } }))]
        public void ParserMixCount(string[] args)
        {


            CommandLinePreferences clp = new CommandLinePreferences(args);

            Assert.True(clp.MinumumUpper == 5 && clp.MinimumLower == 4 && clp.MinimumDigit == 2 && clp.MinimumSpecial == 1);
            //Assert.True(clp.MinumumUpper == 5);
        }

        [Theory]
        [InlineData((new object[] { new string[] { "e=h", "b=32", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=hex", "b=32", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=H", "b=32", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=HEX", "b=32", "u=5", "l=4", "d=2", "s=1" } }))]
        public void ParserHexEncoding(string[] args)
        {
            CommandLinePreferences clp = new CommandLinePreferences(args);
            Assert.True(clp.ByteEncoding == XEncoding.Hex);
        }

        [Theory]
        [InlineData((new object[] { new string[] { "e=b64", "b=32", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=B64", "b=32", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=base64", "b=32", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=BaSe64", "b=32", "u=5", "l=4", "d=2", "s=1" } }))]
        public void ParserBase64Encoding(string[] args)
        {

            CommandLinePreferences clp = new CommandLinePreferences(args);

            Assert.True(clp.ByteEncoding == XEncoding.Base64);
        }

        [Theory]
        [InlineData((new object[] { new string[] { "e=sb64", "b=32", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=SB64", "b=32", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=saFebase64", "b=32", "u=5", "l=4", "d=2", "s=1" } }))]
        [InlineData((new object[] { new string[] { "e=SafEBaSe64", "b=32", "u=5", "l=4", "d=2", "s=1" } }))]
        public void ParserSafeBase64Encoding(string[] args)
        {

            CommandLinePreferences clp = new CommandLinePreferences(args);

            Assert.True(clp.ByteEncoding == XEncoding.SafeBase64);
        }

        //TODO: Test Dice encoding...
        [Theory]
        [InlineData((new object[] { new string[] { "e=dice", "d4=2", "d6=2", "d20=1", } }))]
        [InlineData((new object[] { new string[] { "e=dice", "d4=2", "d6=2", "d20=1", } }))]
        [InlineData((new object[] { new string[] { "e=DiCE", "d10=2", "d6=2", "d20=1", } }))]
        [InlineData((new object[] { new string[] { "e=dICE", "d4=2", "d6=2", "d20=1", } }))]
        [InlineData((new object[] { new string[] { "e=DICE", "d4=2", "d6=2", "d20=1", } }))]
        public void DiceEncoding(string[] args)
        {

            CommandLinePreferences clp = new CommandLinePreferences(args);

            Assert.True(clp.ByteEncoding == XEncoding.Dice);
        }
    }
}
