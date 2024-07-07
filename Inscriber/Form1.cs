using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inscriber
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void openFileClick(object sender, EventArgs e)
        {
            DialogResult openFileResult = openFileDialog.ShowDialog();

            if (openFileResult != DialogResult.OK) { return; }

            FileLoader loader = new FileLoader();
            currentStatus.Text = $"Loading {openFileDialog.SafeFileName}...";
            Application.DoEvents(); // Make sure the status bar updates
            Cursor.Current = Cursors.WaitCursor;
            List<TranslatedString> strings = loader.LoadFile(openFileDialog.FileName);
            String[] row = new string[3];
            
            foreach(TranslatedString s in strings)
            {
                row[0] = s.getId();
                row[1] = s.getOriginal();
                row[2] = s.getTranslation();
                stringsView.Rows.Add(row);
            }

            Cursor.Current = Cursors.Default;
            currentStatus.Text = $"Loaded {loader.StringCount} strings";
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void stringsView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void clearClick(object sender, EventArgs e)
        {
            stringsView.CurrentCell.Value = null;
        }

        private void fillControlSequences(int row)
        {
            if(row < 0 || row >= stringsView.Rows.Count - 1) { return; }
            string currentOriginal = stringsView.Rows[row].Cells[1].Value.ToString();
            string currentTranslation = stringsView.Rows[row].Cells[2].Value.ToString();
            if (currentOriginal == null)
            {
                return;
            }
            if(currentTranslation.IndexOf('#') == -1)
            {
                return;
            }
            List<string> controlCodes = TranslatedString.findControlSequences(currentOriginal);
            if (currentTranslation.Count(c => c == '#') != controlCodes.Count)
            {
                currentStatus.Text = "Wrong control code number";
                return;
            }
            int index = currentTranslation.IndexOf('#');
            for (int i = 0; index != -1; i++)
            {
                currentTranslation = currentTranslation.Remove(index, 1);
                currentTranslation = currentTranslation.Insert(index, controlCodes[i]);
                index = currentTranslation.IndexOf('#');
            }
            stringsView.Rows[row].Cells[2].Value = currentTranslation;
            currentStatus.Text = $"Inserted {controlCodes.Count} control codes";
        }

        private void ctrlSequenceButtonClick(object sender, EventArgs e)
        {
            if(stringsView.CurrentCell == null)
            {
                currentStatus.Text = "Select a cell first";
                return;
            }
            fillControlSequences(stringsView.CurrentCell.RowIndex);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult exportResult = exportDialog.ShowDialog();
            FileExporter exporter = new FileExporter();
            if(exportResult != DialogResult.OK)
            {
                currentStatus.Text = "Export cancelled";
                return;
            }
            List<TranslatedString> strings = new List<TranslatedString>();
            foreach(DataGridViewRow row in stringsView.Rows)
            {
                if (row.Cells[0].Value == null)
                {
                    continue;
                }
                strings.Add(new TranslatedString(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString()));
            }
            currentStatus.Text = $"Exporting...";
            Application.DoEvents(); // Make sure the status bar updates
            Cursor.Current = Cursors.WaitCursor;

            exporter.ExportFile(exportDialog.FileName, strings);

            currentStatus.Text = $"Exported {strings.Count} strings to {exportDialog.FileName}";
            Cursor.Current = Cursors.Default;
        }

        private void autofillAllControlCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int row = 0; row < stringsView.RowCount; row++)
            {
                fillControlSequences(row);
            }
        }

        private bool askForExit()
        {
            string exitMessage = "Do you really want to quit? (Don't forget to save your changes!)";
            if (random.Next(51) == 48)
            {
                exitMessage = "Are you sure you want me to die?";
            }
            DialogResult result = MessageBox.Show(exitMessage, "Inscriber", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(askForExit())
            {
                Application.Exit();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!askForExit())
            {
                e.Cancel = true;
            }
        }
    }
}
