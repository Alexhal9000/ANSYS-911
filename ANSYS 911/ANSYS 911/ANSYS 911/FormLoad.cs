using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ANSYS_911
{
    public partial class FormLoad : Form
    {
        String original_file, load_file;

        public FormLoad()
        {
            InitializeComponent();
        }

        private void FormLoad_Load(object sender, EventArgs e)
        {
            string[] back_items = System.IO.Directory.GetDirectories(Properties.Settings.Default.mainfolder + @"\" + "Backup");
            foreach (string n in back_items)
            {
                String nanana = n.Replace(Properties.Settings.Default.mainfolder + @"\" + "Backup" + @"\", "");
                nanana = nanana.Replace(".db", "");
                comboBox1.Items.Add(nanana);
            }

            string[] proj_items = System.IO.Directory.GetDirectories(Properties.Settings.Default.mainfolder + @"\" + "Projects");
            foreach (string n in proj_items)
            {
                String nanana = n.Replace(Properties.Settings.Default.mainfolder + @"\" + "Projects" + @"\", "");
                nanana = nanana.Replace(".db", "");
                comboBox2.Items.Add(nanana);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_origin.Items.Clear();

            string[] proj_items2 = System.IO.Directory.GetFiles(Properties.Settings.Default.mainfolder + @"\" + "Backup" + @"\" + comboBox1.Text.ToString(),"*.db");
            foreach (string n in proj_items2)
            {
                String nanana = n.Replace(Properties.Settings.Default.mainfolder + @"\" + "Backup" + @"\" + comboBox1.Text.ToString() + @"\", "");
                nanana = nanana.Replace(".db", "");
                comboBox_origin.Items.Add(nanana);
                
            }

        }

        String cbx_proj;
        String cbx_bckfile;

        private void button1_Click(object sender, EventArgs e)
        {
            cbx_proj = comboBox1.Text.ToString();
            cbx_bckfile = comboBox_origin.Text.ToString();
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //get original name
            String path = Properties.Settings.Default.mainfolder + @"\" + "Projects" + @"\" + cbx_proj;
            string[] files = System.IO.Directory.GetFiles(path, "*.db");
            String pathfrom = files[0];
            String backname = files[0].Replace(path + @"\", "");
            backname = backname.Replace(".db", "");
            //copy file
            original_file = Properties.Settings.Default.mainfolder + @"\" + "Projects" + @"\" + cbx_proj + @"\" + backname + ".db";
            load_file = Properties.Settings.Default.mainfolder + @"\" + "Backup" + @"\" + cbx_proj + @"\" + cbx_bckfile + ".db";
            File.Copy(load_file, original_file, true);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String selected_project = comboBox2.Text.ToString(); 
            //get original name
            String path = Properties.Settings.Default.mainfolder + @"\" + "Projects" + @"\" + selected_project;
            string[] files = System.IO.Directory.GetFiles(path, "*.db");
            String pathfrom = files[0];
            String backname = files[0].Replace(path + @"\", "");
            backname = backname.Replace(".db", "");
            //copy file
            String openfilepath = Properties.Settings.Default.mainfolder + @"\" + "Projects" + @"\" + selected_project + @"\" + backname + ".db";

            System.Diagnostics.Process.Start(openfilepath);
            this.Close();
        }
    }
}
