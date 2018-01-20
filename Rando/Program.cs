/* Attributions
 * Mike DePaul
 * https://github.com/mdepaul/Rando-Calrissian.git
 * **/

using System;
using System.Text;
using System.Windows.Forms;

namespace MD.RandoCalrissian
{
    class Program
    {
        private const string ClipboardCopyText = "Copied to clipboard. Hit any key to exit.";
        private const string HelpFile = "Help.txt";

        /// <summary>
        /// rando b=32 e=h
        /// rando b=32 e=b64
        /// rando b=32 e=sb64
        /// rando b=32 e=ulds
        /// rando b=32 e=ulds u=1 l=1 d=3 s=2
        /// rando b=32 m=c:\chars.txt           //Not implemtnted. Custom character mix. No minimums with this one?  We don't know the character mix
        /// rando v = verbose mode              //Show details
        /// rando c = clipboard/copy
        /// rando f=c:\list.txt top=3           //return top three randomized lines from the given CrLf-delimited list
        /// rando e=dice d6 d10=3 d4=5          //A list of die and the number of rolls
        /// rando r=N                           //Repeat N times
        /// 
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            ProcessArguments(args);
        }

        static void ProcessArguments(string[] args)
        {


            CommandLinePreferences clp = new CommandLinePreferences(args);
            if (clp.Help)
            {
                WriteHelpText();
                return;
            }

            if (clp.Verbose)
            {
                Console.WriteLine(clp.ToString());
            }

            string output = ReadOutput(clp, new PrngSHA256());

            if (clp.Clipboard)
            {

                CopyToClipboard(output);
                ShowCopiedText(output);
                Console.WriteLine(ClipboardCopyText);
                Console.ReadKey();
            }
            else
            {

                Console.Out.WriteLine(output);
            }
        }

        static void WriteHelpText()
        {
            string[] lines = System.IO.File.ReadAllLines(HelpFile);
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }

        static void ShowCopiedText(string output)
        {
            Console.Out.WriteLine(output);
            Console.Out.WriteLine();
        }

        static string ReadOutput(CommandLinePreferences clp, IPrng prng)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < clp.Repeat; i++)
            {
                sb.Append(new Rando(clp, prng).Make().Output);
                if (i + 1 != clp.Repeat)
                {
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        [STAThread]
        static void CopyToClipboard(string text)
        {
            Clipboard.SetText(text);
        }
    }
}
