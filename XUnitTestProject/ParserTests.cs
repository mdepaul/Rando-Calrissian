using MD.RandoCalrissian;
using System;
using Xunit;

namespace UnitTests
{
    public class ParserTests
    {

        [Fact]
        public void ParserVerbose()
        {
            string[] args = new string[5];
            args[0] = "e=u";
            args[1] = "b=32";
            args[2] = "u=5";
            args[2] = "V";

            CommandLinePreferences clp = new CommandLinePreferences(args);

            Assert.True(clp.Verbose);
        }

        [Theory]
        [InlineData(new object[] { new string[] { "e=u", "b=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=l", "b=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=d", "b=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=s", "b=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=", "b=32", "u=5", "d=2" } })]
        [InlineData(new object[] { new string[] { "e=ulds", "b=32", "u=5", "d=2" } })]
        public void ParserPasswordEncoding(string[] args)
        {
            CommandLinePreferences clp = new CommandLinePreferences(args as string[]);
            Assert.True(clp.ByteEncoding == XEncoding.Password && clp.IsValid);
        }

        [Fact]
        public void ParserByteCount()
        {
            string[] args = new string[5];
            args[0] = "e=us";
            args[1] = "b=32";
            args[2] = "u=5";
            args[2] = "d=2";

            string[] args2 = new string[args.Length];
            args.CopyTo(args2, 0);
            args2[1] = "byte=12";

            string[] args3 = new string[args.Length];
            args.CopyTo(args3, 0);
            args3[1] = "bytes=24";



            CommandLinePreferences clp = new CommandLinePreferences(args);
            CommandLinePreferences clp2 = new CommandLinePreferences(args2);
            CommandLinePreferences clp3 = new CommandLinePreferences(args3);

            Assert.True(clp.Bytes == 32);
            Assert.True(clp2.Bytes == 12);
            Assert.True(clp3.Bytes == 24);
        }

        [Fact]
        public void ParserMixCount()
        {
            string[] args = new string[5];
            args[0] = "e=ul";
            args[1] = "b=10";
            args[2] = "u=5";
            args[3] = "l=5";

            string[] args2 = new string[args.Length];
            args.CopyTo(args2, 0);
            args2[2] = "u=7";
            args2[3] = "l=0";


            string[] args3 = new string[args.Length];
            args.CopyTo(args3, 0);
            args3[2] = "u=10";
            args3[3] = "l=0";



            CommandLinePreferences clp = new CommandLinePreferences(args);
            CommandLinePreferences clp2 = new CommandLinePreferences(args2);
            CommandLinePreferences clp3 = new CommandLinePreferences(args3);

            Assert.True(clp.MinumumUpper == 5 && clp.MinimumLower == 5);
            Assert.True(clp2.MinumumUpper == 7);
            Assert.True(clp3.MinumumUpper == 10);
        }

        [Fact]
        public void ParserHexEncoding()
        {
            string[] args = new string[5];
            args[0] = "e=h";
            args[1] = "b=32";
            args[2] = "u=5";
            args[2] = "d=2";

            string[] args2 = new string[args.Length];
            args.CopyTo(args2, 0);
            args2[0] = "e=hex";

            CommandLinePreferences clp = new CommandLinePreferences(args);
            CommandLinePreferences clp2 = new CommandLinePreferences(args2);

            Assert.True(clp.ByteEncoding == XEncoding.Hex);
            Assert.True(clp2.ByteEncoding == XEncoding.Hex);
        }

        [Fact]
        public void ParserBase64Encoding()
        {
            string[] args = new string[5];
            args[0] = "e=b64";
            args[1] = "b=32";
            args[2] = "u=5";
            args[2] = "d=2";

            string[] args2 = new string[args.Length];
            args.CopyTo(args2, 0);
            args2[0] = "e=B64";

            string[] args3 = new string[args.Length];
            args.CopyTo(args3, 0);
            args3[0] = "e=bAse64";

            CommandLinePreferences clp = new CommandLinePreferences(args);
            CommandLinePreferences clp2 = new CommandLinePreferences(args2);
            CommandLinePreferences clp3 = new CommandLinePreferences(args3);

            Assert.True(clp.ByteEncoding == XEncoding.Base64);
            Assert.True(clp2.ByteEncoding == XEncoding.Base64);
            Assert.True(clp3.ByteEncoding == XEncoding.Base64);
        }

        [Fact]
        public void ParserSafeBase64Encoding()
        {
            string[] args = new string[5];
            args[0] = "e=sb64";
            args[1] = "b=32";
            args[2] = "u=5";
            args[2] = "d=2";

            string[] args2 = new string[args.Length];
            args.CopyTo(args2, 0);
            args2[0] = "e=sB64";

            string[] args3 = new string[args.Length];
            args.CopyTo(args3, 0);
            args3[0] = "e=SaFebAse64";

            CommandLinePreferences clp = new CommandLinePreferences(args);
            CommandLinePreferences clp2 = new CommandLinePreferences(args2);
            CommandLinePreferences clp3 = new CommandLinePreferences(args3);

            Assert.True(clp.ByteEncoding == XEncoding.SafeBase64);
            Assert.True(clp2.ByteEncoding == XEncoding.SafeBase64);
            Assert.True(clp3.ByteEncoding == XEncoding.SafeBase64);
        }

        //TODO: Test Dice encoding...
        [Fact]
        public void DiceEncoding()
        {
            string[] args = new string[5];
            args[0] = "e=dice";
            args[1] = "d4=2";
            args[2] = "d6=2";
            args[2] = "d20=3";

            CommandLinePreferences clp = new CommandLinePreferences(args);

            Assert.True(clp.ByteEncoding == XEncoding.Dice);
        }
    }
}
