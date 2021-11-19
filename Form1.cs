using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileWatcher
{
    public partial class Form1 : Form
    {
        private List<string> Logs = new List<string>();

        public Form1()
        {
            InitializeComponent();
            FileSystemWatcher = new FileSystemWatcher();

            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Tick += (s, e) => {
                UpdateTextBox();
            };
            timer.Start();

            FileSystemWatcher.Renamed += (sender, args) =>
            {
                Logs.Add("Renamed: " + args.OldFullPath);
                Logs.Add("To: " + args.FullPath);
            };
            FileSystemWatcher.Deleted += (sender, args) =>
            {
                Logs.Add("Deleted: " + args.FullPath);
            };
            FileSystemWatcher.Created += (sender, args) =>
            {
                Logs.Add("Created: " + args.FullPath);
            };
            FileSystemWatcher.Changed += (sender, args) =>
            {
                Logs.Add("Changed: " + args.FullPath);
            };

            FileSystemWatcher.IncludeSubdirectories = false;
            FileSystemWatcher.Path = "C:\\";
            FileSystemWatcher.EnableRaisingEvents = true;
        }

        public FileSystemWatcher FileSystemWatcher;

        
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileSystemWatcher.Path = folderBrowserDialog.SelectedPath;
            }
            else
            {
                MessageBox.Show("User abort!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FileSystemWatcher.IncludeSubdirectories = checkBox1.Checked;
            if (checkBox1.Checked)
            {
                Logs.Add("Turned on watching of subdirectories");
            }
            else
            {
                Logs.Add("Turned off watching of subdirectories");
            }

            textBox1.Lines = Logs.ToArray();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (Logs.Count > 30)
            {
                Logs.RemoveAt(0);
                textBox1.Lines = Logs.ToArray();
            }
        }

        private void UpdateTextBox()
        {
            textBox1.Lines = Logs.ToArray();
        }
    }
}
