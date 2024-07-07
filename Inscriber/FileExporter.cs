using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inscriber
{
    internal class FileExporter
    {
        public void ExportFile(string filename, List<TranslatedString> strings, string[] fileHeader)
        {
            if(fileHeader.Length != 4)
            {
                throw new ArgumentException("Invalid file header");
            }
            StreamWriter writer = new StreamWriter(filename);
            foreach(string headerLine in fileHeader) {
                writer.WriteLine(headerLine);
            }
            foreach(TranslatedString trString in strings)
            {
                writer.WriteLine(trString.toCSV());
                writer.WriteLine();
            }
            writer.Close();
        }
    }
}
