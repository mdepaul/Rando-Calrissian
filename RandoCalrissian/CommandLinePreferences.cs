/* Attributions
 * Mike DePaul
 * https://github.com/mdepaul/Rando-Calrissian.git
 * **/

using System;
using System.Linq;
using System.Text;


namespace MD.RandoCalrissian
{
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
        public bool IsValid { get; set; }

        public bool Calrissian { get; set; }

        public ArgumentException ArgumentException { get; set; }

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
            foreach (var item in args)
            {
                if (String.IsNullOrEmpty(item))
                    continue;

                if (item.StartsWith("Calrissian", StringComparison.CurrentCultureIgnoreCase))
                {
                    IsValid = true;
                    Calrissian = true;
                    //break;
                }

                if (item.StartsWith("H", StringComparison.CurrentCultureIgnoreCase) || item.StartsWith("Help", StringComparison.CurrentCultureIgnoreCase))
                {
                    Help = true;
                    break;
                }

                if (item.StartsWith("b=", StringComparison.CurrentCultureIgnoreCase) || item.StartsWith("byte=", StringComparison.CurrentCultureIgnoreCase) || item.StartsWith("bytes=", StringComparison.CurrentCultureIgnoreCase))
                    Bytes = ParseArgument(item, Bytes);

                if (item.StartsWith("e=", StringComparison.CurrentCultureIgnoreCase) || item.StartsWith("encoding=", StringComparison.CurrentCultureIgnoreCase))
                {
                    try
                    {
                        ByteEncoding = ParseXEncoding(item);
                    }
                    catch (ArgumentException e)
                    {
                        ArgumentException = e;
                        return;
                    }
                    UseUpper = ByteEncoding == XEncoding.Password ? ParseArgument(item, "U") : false;
                    UseLower = ByteEncoding == XEncoding.Password ? ParseArgument(item, "L") : false;
                    UseSpecial = ByteEncoding == XEncoding.Password ? ParseArgument(item, "S") : false;
                    UseDigit = ByteEncoding == XEncoding.Password ? ParseArgument(item, "D") : false;
                }

                #region Passwords
                if (item.StartsWith("u=", StringComparison.CurrentCultureIgnoreCase) || item.StartsWith("upper=", StringComparison.CurrentCultureIgnoreCase))
                    MinumumUpper = ParseArgument(item, MinumumUpper);

                if (item.StartsWith("l=", StringComparison.CurrentCultureIgnoreCase) || item.StartsWith("lower=", StringComparison.CurrentCultureIgnoreCase))
                    MinimumLower = ParseArgument(item, MinimumLower);

                if (item.StartsWith("d=", StringComparison.CurrentCultureIgnoreCase) || item.StartsWith("digit=", StringComparison.CurrentCultureIgnoreCase) || item.StartsWith("digits=", StringComparison.CurrentCultureIgnoreCase))
                    MinimumDigit = ParseArgument(item, MinimumDigit);

                if (item.StartsWith("s=", StringComparison.CurrentCultureIgnoreCase) || item.StartsWith("special=", StringComparison.CurrentCultureIgnoreCase))
                    MinimumSpecial = ParseArgument(item, MinimumSpecial);
                #endregion

                #region Dice
                if (item.StartsWith("d4", StringComparison.CurrentCultureIgnoreCase))
                    D4 = ParseArgument(item, 1);

                if (item.StartsWith("d6", StringComparison.CurrentCultureIgnoreCase))
                    D6 = ParseArgument(item, 1);

                if (item.StartsWith("d8", StringComparison.CurrentCultureIgnoreCase))
                    D8 = ParseArgument(item, 1);

                if (item.StartsWith("d10", StringComparison.CurrentCultureIgnoreCase))
                    D10 = ParseArgument(item, 1);

                if (item.StartsWith("d12", StringComparison.CurrentCultureIgnoreCase))
                    D12 = ParseArgument(item, 1);

                if (item.StartsWith("d20", StringComparison.CurrentCultureIgnoreCase))
                    D20 = ParseArgument(item, 1);
                #endregion

                if (item.StartsWith("F", StringComparison.CurrentCultureIgnoreCase) && item.Contains("="))
                    FilePath = ParseArgument(item);

                if (item.StartsWith("top", StringComparison.CurrentCultureIgnoreCase) && item.Contains("="))
                    FileTop = ParseArgument(item, FileTop);

                if (item.StartsWith("R", StringComparison.CurrentCultureIgnoreCase) && item.Contains("="))
                    Repeat = ParseArgument(item, Repeat);

                if (item.StartsWith("C", StringComparison.CurrentCultureIgnoreCase) && item.StartsWith("Calrissian", StringComparison.CurrentCultureIgnoreCase) == false)
                    Clipboard = true;

                if (item.StartsWith("V", StringComparison.CurrentCultureIgnoreCase) || item.StartsWith("Verbose", StringComparison.CurrentCultureIgnoreCase))
                    Verbose = true;

                //if (item.StartsWith("K", StringComparison.CurrentCultureIgnoreCase))
                //    keepOpen = true;

                //if (item.StartsWith("?"))
                //    help = true;
            }

            if (Calrissian)
            {
                IsValid = true;
                return;
            }

            if (this.ByteEncoding == XEncoding.Dice)
            {
                IsValid = true;
                return;
            }

            if (!String.IsNullOrEmpty(FilePath))
            {
                ReadFile = true;
                IsValid = true;
                return;
            }

            if (Bytes < 0)
            {
                IsValid = false;
                ArgumentException = new ArgumentException(String.Format("{0} is an invalid number of bytes, a positive integer is required", Bytes));
                return;
            }

            if ((MinumumUpper + MinimumLower + MinimumSpecial + MinimumDigit) > Bytes)
            {
                IsValid = false;
                ArgumentException = new ArgumentException(String.Format("The parameters require {0} bytes, but the desired length is only {1}", MinumumUpper + MinimumLower + MinimumSpecial + MinimumDigit, Bytes));
                return;
            }

            if (ByteEncoding == XEncoding.Password)
            {
                MinumumUpper = UseUpper ? MinumumUpper : 0;
                MinimumLower = UseLower ? MinimumLower : 0;
                MinimumDigit = UseDigit ? MinimumDigit : 0;
                MinimumSpecial = UseSpecial ? MinimumSpecial : 0;

                MinumumUpper = UseUpper && MinumumUpper == 0 ? 1 : MinumumUpper;
                MinimumLower = UseLower && MinimumLower == 0 ? 1 : MinimumLower;
                MinimumDigit = UseDigit && MinimumDigit == 0 ? 1 : MinimumDigit;
                MinimumSpecial = UseSpecial && MinimumSpecial == 0 ? 1 : MinimumSpecial;
            }

            if (ByteEncoding == XEncoding.Dice)
            {
                //TODO: What?
            }

            if ((ByteEncoding == XEncoding.Hex || ByteEncoding == XEncoding.Hex) && Bytes < 1)
            {
                IsValid = false;
                ArgumentException = new ArgumentException(String.Format("{0} bytes is an invalid number.", Bytes));
                return;
            }
            IsValid = true;
        }


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

        XEncoding ParseXEncoding(string[] args)
        {
            XEncoding encoding = XEncoding.Password;
            foreach (var item in args)
            {
                if (String.IsNullOrEmpty(item))
                    continue;


            }
            return encoding;
        }


        int ParseArgument(string arg, int current)
        {
            int returnValue = current;
            if (!arg.Contains("="))
                return current;

            string[] split = arg.Split("=".ToCharArray());
            string val = split[1];
            int newValue;
            if (Int32.TryParse(val, out newValue))
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
