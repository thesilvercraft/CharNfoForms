using SilverFormsUtils;
using System.Diagnostics;
using System.Text.Json;
using System.Unicode;

namespace CharNfoForms
{
    public partial class CharNfoForm : Form
    {
        public CharNfoForm()
        {
            InitializeComponent();
            if (File.Exists("config.json"))
            {
                using Stream stream = File.OpenRead("config.json");
                StreamReader reader = new(stream);
                string content = reader.ReadToEnd();
                reader.Dispose();
                var e = JsonSerializer.Deserialize<Config>(content);
                checkBox1.Checked = e.StayOnTop;
                checkBox2.Checked = e.DarkishMode;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Explain();
        }

        private void Explain()
        {
            richTextBox1.Text = string.Empty;
            foreach (char character in textBox1.Text)
            {
                richTextBox1.Text += GetCodePointInfo(character) + Environment.NewLine;
            }
        }

        private static string GetCodePointInfo(int codePoint)
        {
            UnicodeCharInfo charInfo = UnicodeInfo.GetCharInfo(codePoint);
            return $"{UnicodeInfo.GetDisplayText(charInfo)}\tU+{codePoint:X4}\t{charInfo.Name ?? charInfo.OldName}\t{charInfo.Category}\thttps://www.fileformat.info/info/unicode/char/{codePoint:X4}";
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = checkBox1.Checked;
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Explain();
                e.Handled = true;
            }
        }

        private void CharNfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            using StreamWriter streamWriter = new("config.json");
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            streamWriter.Write(JsonSerializer.Serialize(new Config { StayOnTop = checkBox1.Checked, DarkishMode = checkBox2.Checked }, options));
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.UseDarkModeBar(checkBox2.Checked);
            this.UseDarkModeForThingsInsideOfForm(checkBox2.Checked, true);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = e.LinkText
            };
            Process.Start(psi);
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            button1.Location = new(e.X - 85, button1.Location.Y);
        }
    }
}