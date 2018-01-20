/* Attributions
 * Mike DePaul
 * https://github.com/mdepaul/Rando-Calrissian.git
 * **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MD.RandoCalrissian
{
    public class KeyVal
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public KeyVal()
        {
            Key = "";
            Value = "";
        }
    }

    public class CommandLinePreferences
    {
        public XEncoding ByteEncoding { get; set; }
        public int Bytes { get; set; }

        public bool Help { get; set; }

        public bool UseUpper { get; set; }
        public bool UseLower { get; set; }
        public bool UseDigit { get; set; }
        public bool UseSpecial { get; set; }

        public int MinumumUpper { get; set; }
        public int MinimumLower { get; set; }
        public int MinimumDigit { get; set; }
        public int MinimumSpecial { get; set; }


        public int D4 { get; set; }
        public int D6 { get; set; }
        public int D8 { get; set; }
        public int D10 { get; set; }
        public int D12 { get; set; }
        public int D20 { get; set; }

        public bool ReadFile { get; set; }
        public string FilePath { get; set; }
        public int FileTop { get; set; }

        public bool Verbose { get; set; }
        public bool Clipboard { get; set; }
        public int Repeat { get; set; }

        public bool Calrissian { get; set; }

        public ArgumentException ArgumentException { get; set; }

        Dictionary<string, string> Arguments { get; set; }

        //TODO: Make this into a Factory that delivers specific parsers... ?

        /// <summary>
        /// Parses the arguments and creates a CommandLinePreferences object.
        /// Yeah, this method is too busy.
        /// </summary>
        /// <throws>
        ///     ArgumentException
        /// </throws>
        /// <param name="args"></param>
        public CommandLinePreferences(string[] args)
        {
            Repeat = 1;
            Arguments = ParseArgs(args);

            GetByteEncoding(Arguments);

            switch (ByteEncoding)
            {
                case XEncoding.Unknown:
                    break;
                case XEncoding.Help:
                    break;
                case XEncoding.Password:
                    ParsePassword(Arguments);
                    break;
                case XEncoding.Calrissian:
                    break;
                case XEncoding.Dice:
                    ParseDice(Arguments);
                    break;
                case XEncoding.File:
                    ParseFile(Arguments);
                    break;
                case XEncoding.Hex:
                    ParseHex(Arguments);
                    break;
                case XEncoding.Base64:
                    ParseBase64(Arguments);
                    break;
                case XEncoding.SafeBase64:
                    ParseSafeBase64(Arguments);
                    break;
                default:
                    break;
            }


            ParseRepeat(Arguments);
            ParseVerbose(Arguments);
            ParseClipboard(Arguments);
        }

        private void GetByteEncoding(Dictionary<string, string> args)
        {
            if (args.Keys.Contains("calrissian"))
            {
                ByteEncoding = XEncoding.Calrissian;
                return;
            }

            if (args.Keys.Contains("?"))
            {
                ByteEncoding = XEncoding.Help;
            }


            if (args.Keys.Contains("f"))
            {
                ByteEncoding = XEncoding.Help;
            }

            if (args.Keys.Contains("file"))
            {
                ByteEncoding = XEncoding.Help;
            }

            if (args.Keys.Contains("e"))
            {
                if (args["e"] == "dice")
                {
                    ByteEncoding = XEncoding.Dice;
                    return;
                }

                if (args["e"] == "h" || args["e"] == "hex")
                {
                    ByteEncoding = XEncoding.Hex;
                    return;
                }

                if (args["e"] == "b64" || args["e"] == "base64")
                {
                    ByteEncoding = XEncoding.Base64;
                    return;
                }

                if (args["e"] == "sb64" || args["e"] == "safebase64")
                {
                    ByteEncoding = XEncoding.SafeBase64;
                    return;
                }

                if (args["e"].Contains("u") || args["e"].Contains("l") || args["e"].Contains("d") || args["e"].Contains("s"))
                {
                    ByteEncoding = XEncoding.Password;
                    return;
                }
            }

            ByteEncoding = XEncoding.Unknown;

        }

        private Dictionary<string, string> ParseArgs(string[] args)
        {
            Dictionary<string, string> arguments = new Dictionary<string, string>();
            KeyVal keyVal;
            foreach (var arg in args)
            {
                if (arg != null)
                {
                    keyVal = ParseArg(arg);
                    if (arguments.ContainsKey(keyVal.Key) == false)     //Only keep the first arg value
                    {
                        arguments.Add(keyVal.Key, keyVal.Value);
                    }
                }
            }

            return arguments;
        }

        private static KeyVal ParseArg(string arg)
        {
            arg = arg.ToLower();
            KeyVal keyVal = new KeyVal();
            string[] p = arg.Split("=".ToCharArray());
            if (p.Length > 1)
            {
                keyVal.Key = p[0].Trim();
                keyVal.Value = arg.Substring(p[0].Length);
                keyVal.Value = keyVal.Value.Replace("=", "").Trim();
            }
            else
            {
                keyVal.Key = arg;
            }

            return keyVal;
        }

        #region Common
        private void ParseCharacterMix(Dictionary<string, string> args)
        {
            string mix = "";
            if (args.ContainsKey("e"))
            {
                mix = args["e"];
            }

            UseUpper = mix.Contains("u");
            UseLower = mix.Contains("l");
            UseDigit = mix.Contains("d");
            UseSpecial = mix.Contains("s");
        }
        #endregion

        #region Parse Operations
        private void ParsePassword(Dictionary<string, string> args)
        {
            ParseByteLength(args);
            ParseCharacterMix(args);
            ParseMinimumCharacters(args);
        }

        private void ParseRepeat(Dictionary<string, string> args)
        {
            string repeat = "";
            if (args.ContainsKey("r"))
            {
                repeat = args["r"];
            }

            if (int.TryParse(repeat, out int oRepeat))
            {
                Repeat = oRepeat;
            }
        }


        private void ParseVerbose(Dictionary<string, string> args)
        {
            Verbose = args.ContainsKey("v");
        }

        private void ParseClipboard(Dictionary<string, string> args)
        {
            Clipboard = args.ContainsKey("c");
        }

        private void ParseByteLength(Dictionary<string, string> args)
        {
            Bytes = 0;
            if (args.ContainsKey("b") == false && args.ContainsKey("byte") == false && args.ContainsKey("bytes") == false)
                return;

            string bytes = "";
            if (args.ContainsKey("b"))
                bytes = args["b"];

            if (args.ContainsKey("byte"))
                bytes = args["byte"];

            if (args.ContainsKey("bytes"))
                bytes = args["bytes"];

            if (int.TryParse(bytes, out int iBytes))
            {
                Bytes = iBytes;
            }
        }

        private void ParseMinimumCharacters(Dictionary<string, string> args)
        {
            MinumumUpper = ParseArgValue(args, "u");
            MinimumLower = ParseArgValue(args, "l");
            MinimumDigit = ParseArgValue(args, "d");
            MinimumSpecial = ParseArgValue(args, "s");
        }

        private void ParseDice(Dictionary<string, string> args)
        {
            D4 = ParseArgValue(args, "d4");
            D6 = ParseArgValue(args, "d6");
            D8 = ParseArgValue(args, "d8");
            D10 = ParseArgValue(args, "d10");
            D12 = ParseArgValue(args, "d12");
            D20 = ParseArgValue(args, "d20");
        }

        private int ParseArgValue(Dictionary<string, string> args, string diceKey)
        {
            int count = 0;
            if (args.ContainsKey(diceKey) == false)
                return count;

            if (int.TryParse(args[diceKey], out int iCount))
            {
                count = iCount;
            }

            return count;
        }

        private void ParseFile(Dictionary<string, string> args)
        {
            ParseFileName(args);
            ParseFileTop(args);
        }

        private void ParseFileName(Dictionary<string, string> args)
        {
            if (args.ContainsKey("f"))
            {
                FilePath = args["f"];
            }
        }

        private void ParseFileTop(Dictionary<string, string> args)
        {
            string top = "";
            if (args.ContainsKey("top"))
            {
                top = args["top"];
            }

            if (int.TryParse(top, out int oTop))
            {
                FileTop = oTop;
            }
        }

        private void ParseHex(Dictionary<string, string> args)
        {
            ParseByteLength(args);
        }

        private void ParseBase64(Dictionary<string, string> args)
        {
            ParseByteLength(args);
        }

        private void ParseSafeBase64(Dictionary<string, string> args)
        {
            ParseByteLength(args);
        }

        #endregion

        bool IsCalrissian(string[] args)
        {
            foreach (var item in args)
            {
                if (String.IsNullOrEmpty(item))
                    continue;

                if (item.StartsWith("Calrissian", StringComparison.CurrentCultureIgnoreCase))
                {
                    Calrissian = true;
                    return true;
                }
            }
            return false;
        }

        int ParseArgument(string arg, int current)
        {
            int returnValue = current;
            if (!arg.Contains("="))
                return current;

            string[] split = arg.Split("=".ToCharArray());
            string val = split[1];
            if (Int32.TryParse(val, out int newValue))
                returnValue = newValue;

            return returnValue;
        }

        private bool ParseArgument(string arg, string p)
        {
            string[] split = arg.Split("=".ToCharArray());
            string val = split[1];
            return val.ToLower().Contains(p.ToLower());
        }

        private string ParseArgument(string arg)
        {
            string[] split = arg.Split("=".ToCharArray());
            return split[1];
        }

        XEncoding ParseXEncoding(string arg)
        {
            string validPassword = "ulds";
            string[] split = arg.Split("=".ToCharArray());
            string val = split[1];

            if (val.Equals("Hex", StringComparison.CurrentCultureIgnoreCase))
            {
                return XEncoding.Hex;
            }
            if (val.Equals("h", StringComparison.CurrentCultureIgnoreCase))
            {
                return XEncoding.Hex;
            }

            if (val.Equals("Base64", StringComparison.CurrentCultureIgnoreCase))
            {
                return XEncoding.Base64; ;
            }
            if (val.Equals("b64", StringComparison.CurrentCultureIgnoreCase))
            {
                return XEncoding.Base64; ;
            }

            if (val.Equals("SafeBase64", StringComparison.CurrentCultureIgnoreCase))
            {
                return XEncoding.SafeBase64;
            }
            if (val.Equals("sb64", StringComparison.CurrentCultureIgnoreCase))
            {
                return XEncoding.SafeBase64;
            }

            if (val.Equals("dice", StringComparison.CurrentCultureIgnoreCase))
            {
                return XEncoding.Dice;
            }

            bool validPasswordChars = true;
            foreach (var item in val.ToCharArray())
            {
                if (!validPassword.ToCharArray().Contains(item))
                {
                    validPasswordChars = false;
                }
            }
            if (validPasswordChars)
            {
                return XEncoding.Password;
            }
            throw new ArgumentException("\"{0}\" is not a valid encoding.", val);
        }

        public bool IsValid()
        {
            if (ByteEncoding == XEncoding.Unknown)
            {
                return false;
            }

            if (Repeat < 1)
            {
                return false;
            }

            if (ByteEncoding == XEncoding.Hex)
            {
                return Bytes > 0;
            }

            if (ByteEncoding == XEncoding.Base64)
            {
                return Bytes > 0;
            }

            if (ByteEncoding == XEncoding.SafeBase64)
            {
                return Bytes > 0;
            }


            if (ByteEncoding == XEncoding.Password)
            {
                if (Bytes < 1) return false;

                if (UseUpper == false && UseLower == false && UseSpecial == false && UseDigit == false)
                {
                    return false;
                }

                if ((MinumumUpper + MinimumLower + MinimumDigit + MinimumSpecial) > Bytes)
                {
                    return false;
                }

                return true;
            }

            //Check depending on settings...
            return false;
        }

        public override string ToString()
        {

            if (Calrissian)
                return "Rando Calrissian";

            if (ReadFile)
                return String.Format("Opening file \"{0}\", reading {1} lines.", FilePath, FileTop);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(String.Format("Encoding={0}", ByteEncoding.ToString()));

            if (ByteEncoding == XEncoding.Dice)
            {
                if (D4 > 0)
                    sb.AppendLine(String.Format("D4  rolls={0}", D4));

                if (D6 > 0)
                    sb.AppendLine(String.Format("D6  rolls={0}", D6));

                if (D8 > 0)
                    sb.AppendLine(String.Format("D8  rolls={0}", D8));

                if (D10 > 0)
                    sb.AppendLine(String.Format("D10 rolls={0}", D10));

                if (D12 > 0)
                    sb.AppendLine(String.Format("D12 rolls={0}", D12));

                if (D20 > 0)
                    sb.AppendLine(String.Format("D20 rolls={0}", D20));
            }
            else
            {
                sb.AppendLine(String.Format("Bytes={0}", Bytes));
            }

            if (ByteEncoding == XEncoding.Password)
            {
                if (UseUpper)
                    sb.AppendLine(String.Format("Upper={0}; Minimum={1}", "Y", MinumumUpper));

                if (UseLower)
                    sb.AppendLine(String.Format("Lower={0}; Minimum={1}", "Y", MinimumLower));

                if (UseDigit)
                    sb.AppendLine(String.Format("Digits={0}; Minimum={1}", "Y", MinimumDigit));

                if (UseSpecial)
                    sb.AppendLine(String.Format("Special={0}; Minimum={1}", "Y", MinimumSpecial));
            }

            return sb.ToString();
        }
    }
}
