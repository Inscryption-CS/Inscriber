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
        public List<TranslatedString> LoadFile(string filename)
        {
            List<TranslatedString> strings = new List<TranslatedString>();
            Console.WriteLine($"Loading {filename}...");
            try
            {
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
                    string[] values = line.Split(';');
                    strings.Add(new TranslatedString(values[0], values[1], values[2]));
                    line = reader.ReadLine();
                }
                Console.WriteLine($"Loaded {stringCount} strings");

            } catch (Exception ex)
            {
                MessageBox.Show($"Error reading file ({ex.Message})", "Inscriber Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return strings;
        }
    }
}
