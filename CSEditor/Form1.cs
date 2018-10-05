using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgramLibrary;

namespace Editor
{
    public partial class Editor : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        static string textBoxText;        
        static string selectedFile;
        static string selectedFilePath;


        public Editor()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // No JS errors
            webBrowser1.ScriptErrorsSuppressed = true;



        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Saving file and opening it in the browser!
            try
            {
                SaveFile.Save(selectedFile, textBox1.Text);
                webBrowser1.Navigate(selectedFile);

            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong!");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Opening File
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog() { Multiselect = false })
                {

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        OpenFile.Open(openFileDialog.FileName);
                        string filename = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf('\\') + 1);
                        selectedFile = openFileDialog.FileName;

                        textBox1.Text = OpenFile.text;
                        textBoxText = textBox1.Text;

                        selectedFilePath = selectedFile.Substring(0, selectedFile.LastIndexOf('\\'));

                        webBrowser1.Navigate(selectedFile);
                        label1.Text = filename;

                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong!");
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label1_Click(object sender, EventArgs e)
        {

            Process.Start("explorer.exe", selectedFilePath);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            webBrowser1.Navigate(new Uri("about:blank"));
            selectedFile = null;
            selectedFilePath = null;
            label1.Text = "Start";
        }


    }
}
