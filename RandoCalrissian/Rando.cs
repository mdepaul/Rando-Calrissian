/* Attributions
 * Mike DePaul
 * https://github.com/mdepaul/Rando-Calrissian.git
 * **/

using System;
using System.Collections.Generic;

namespace MD.RandoCalrissian
{
    public class Rando
    {
        //via http://www.imdb.com/title/tt0080684/characters/nm0001850?ref_=tt_cl_t4
        readonly string[] Quotes = new string[]
        {
            "They told me they fixed it! I \"trusted\" them to \"fix\" it! It's not my fault.",
            "We'll last longer than we will against that Death Star! And we might just take a few of them with us!",
            "What have you done to my ship?",
            "This deal is getting worse all the time!",
            "Yes, he's alive, and in perfect hibernation.",
            "I had no choice. They arrived right before you did. I'm sorry.",
            "How you doin' Chewbacca? Still hanging around with this loser?",
            "That blast came from the Death Star! That thing's operational!",
            "But how could they be jamming us if they don't know... if we're coming?",
            "Break off the attack! The shield is still up!",
            "Pull up! All craft, pull up!",
            "Yes, I said \"closer\"! Move as close as you can, and engage those Star Destroyers at point blank range!",
            "Don't worry, my friend's down there. He'll have that shield down in time. Or this'll be the shortest offensive of all time.",
            "No, wait! I thought you were blind!",
            "Up a little higher! Just a little higher!",
            "We're on our way, Red group, Gold group, all fighters follow me. Ha ha ha, I told you they\'d do it!",
            "You're being put into carbon-freeze.",
            "Why you slimy, double-crossing, no-good swindler. You've got a lot of guts coming here, after what you pulled.",
            "You look absolutely beautiful. You truly belong here with us among the clouds.",
            "Han will have that shield down. We've got to give him more time!"
        };

        //Crypto CryptoProvider;
        IPrng Prng;
        CommandLinePreferences Clp;
        private string outPut;
        public string Output
        {
            get { return outPut; }
        }

        private Rando()
        {
        }
        public Rando(CommandLinePreferences clp, IPrng prng)
        {
            Clp = clp;
            Prng = prng;
            //CryptoProvider = new Crypto(prng);
        }
        public Rando Make()
        {
            if (!Clp.IsValid)
            {
                outPut = Clp.ArgumentException.Message;
                return this;
            }

            if (Clp.Calrissian)
            {
                outPut = Calrissian();
                return this;
            }

            if (Clp.ReadFile)
            {
                FileReader FileReader = new FileReader(Prng, Clp.FilePath, Clp.FileTop);
                List<string> list = FileReader.GetResult();
                foreach (var item in list)
                {
                    if (!String.IsNullOrEmpty(outPut))
                        outPut += "\r\n";
                    outPut += item;
                }
                return this;
            }

            if (Clp.ByteEncoding == XEncoding.Dice)
            {
                DiceMaker Dice = new DiceMaker(Prng);

                int diceTotal = 0;
                diceTotal += Dice.Roll(numberOfD4: Clp.D4);
                diceTotal += Dice.Roll(numberOfD6: Clp.D6);
                diceTotal += Dice.Roll(numberOfD8: Clp.D8);
                diceTotal += Dice.Roll(numberOfD10: Clp.D10);
                diceTotal += Dice.Roll(numberOfD12: Clp.D12);
                diceTotal += Dice.Roll(numberOfD20: Clp.D20);
                outPut = diceTotal.ToString();
            }

            if (Clp.ByteEncoding == XEncoding.Hex)
            {
                outPut = Prng.GetRandomBytes(Clp.Bytes).ToHex();
            }

            if (Clp.ByteEncoding == XEncoding.Base64)
            {
                outPut = Prng.GetRandomBytes(Clp.Bytes).ToBase64String();
            }
            if (Clp.ByteEncoding == XEncoding.SafeBase64)
            {
                outPut = Prng.GetRandomBytes(Clp.Bytes).ToSafeBase64String();
            }
            if (Clp.ByteEncoding == XEncoding.Password)
            {
                CharacterMix mix = new CharacterMix
                {
                    UseDigits = Clp.UseDigit,
                    UseLower = Clp.UseLower,
                    UseSpecial = Clp.UseSpecial,
                    UseUpper = Clp.UseUpper
                };
                outPut = new PasswordMaker(Prng).Make(Clp.Bytes, mix, Clp.MinumumUpper, Clp.MinimumLower, Clp.MinimumDigit, Clp.MinimumSpecial);
            }
            return this;
        }

        public string Calrissian()
        {
            return new RandomizedList<string>(Prng).Add(Quotes)[0];
        }

        private string Verbose(CommandLinePreferences clp)
        {
            return clp.ToString();
        }
    }
}
