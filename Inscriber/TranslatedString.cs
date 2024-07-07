using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inscriber
{
    internal class TranslatedString
    {
        String id;
        String original;
        String translation;
        public TranslatedString(String id, String original, String translation) {
            this.id = id;
            this.original = original;
            this.translation = translation;
        }
        public String toCSV()
        {
            return $"{id};{original};{translation}";
        }
        public String getId()
        {
            return id;
        }
        public String getOriginal()
        {
            return original;
        }
        public String getTranslation()
        {
            return translation;
        }
        public static List<String> findControlSequences(string str)
        {
            List<String> sequences = new List<String>();
            MatchCollection matches = Regex.Matches(str, @"\[.*?\]");
            foreach (Match m in matches)
            {
                sequences.Add(m.Value);
            }
            return sequences;
        }
    }
}
