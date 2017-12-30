/* Attributions
 * Mike DePaul
 * https://github.com/mdepaul/Rando-Calrissian.git
 * **/

using System;
using System.Collections.Generic;
using System.IO;

namespace MD.RandoCalrissian
{
    class FileReader
    {
        IPrng Prng;
        RandomizedList<string> list;
        List<string> result;
        private FileReader()
        {
        }
        public FileReader(IPrng prng, string filepath, int numberToReturn, bool returnWhatsAvailable = false)
        {
            Prng = prng;
            list = new RandomizedList<string>(Prng);
            if (!System.IO.File.Exists(filepath))
                throw new FileNotFoundException(String.Format("\"{0}\" was not found.", filepath));

            string[] lines = System.IO.File.ReadAllLines(filepath);
            foreach (var line in lines)
            {
                if (!String.IsNullOrEmpty(line))
                    list.Add(line);
            }

            if (!returnWhatsAvailable && list.Count < numberToReturn)
                throw new ArgumentOutOfRangeException(String.Format("Requested {0} items, but only {1} are available", numberToReturn, list.Count));

            result = new List<string>();
            int addIndex = 0;
            while (result.Count < numberToReturn && addIndex < list.Count)
            {
                result.Add(list[addIndex]);
                addIndex++;
            }
        }

        public List<string> GetResult()
        {
            return result;
        }
    }
}
