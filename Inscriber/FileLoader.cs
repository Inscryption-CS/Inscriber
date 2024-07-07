using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inscriber
{
    internal class FileLoader
    {
        public int StringCount;
        private TranslatedString parseCSVLine(string s)
        {
            string[] parts = s.Split(',');
            if(parts.Length == 3)
            {
                return new TranslatedString(parts[0].Trim('"'), parts[1].Trim('"'), parts[2].Trim('"'));
            }

            string ID = parts[0].Trim('"');
            string original = "";
            string translation = "";

            int startIndex = s.IndexOf(',') + 2;
            int endIndex = startIndex+1;
            while (s[endIndex] != '"') { endIndex++; }
            original = s.Substring(startIndex, endIndex - startIndex);
            translation = s.Substring(endIndex + 3, s.Length - endIndex - 4);


            return new TranslatedString(ID, original, translation);
        }
        public List<TranslatedString> LoadFile(string filename)
        {
            List<TranslatedString> strings = new List<TranslatedString>();
            Console.WriteLine($"Loading {filename}...");
            /*try
            {*/
                StreamReader reader = new StreamReader(filename);
                string line = "";
                int stringCount = 0;

                line = reader.ReadLine(); 
                while (line != null)
                {
                    if(line == "") {
                        line = reader.ReadLine();
                        continue; 
                    }

                    StringCount++;
                    strings.Add(parseCSVLine(line));
                    line = reader.ReadLine();
                }
                Console.WriteLine($"Loaded {stringCount} strings");

            /*} catch (Exception ex)
            {
                MessageBox.Show($"Error reading file ({ex.Message})", "Inscriber Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
            return strings;
        }
    }
}
