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
    public partial class Form1 : Form
    {
        string newfolder;
        string thefolder;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.Opacity = 0.0;
            splashscreen fs = new splashscreen();
            fs.Show();

            //layout

            int x, y;
            x = Screen.FromControl(this).Bounds.Width;
            y = Screen.FromControl(this).Bounds.Height;
            this.Width = x;
            x = 0;
            y = y - (this.Height) - (y / 27) - 7;
            this.Location = new Point(x,y);

            //folders
            thefolders();

            //update bar
            progressBar1.Maximum = (int)(Properties.Settings.Default.maxhdd) * 10;
            updatehdd();

            // combobox

            getcombobox();

            //config autosave
            autosave();

            
        }

        public void getcombobox()
        {
            comboBox_folder.Items.Clear();
            string[] proj_items = System.IO.Directory.GetDirectories(Properties.Settings.Default.mainfolder + @"\" + "Projects");
            foreach (string n in proj_items)
            {
                String nanana = n.Replace(Properties.Settings.Default.mainfolder + @"\" + "Projects" + @"\", "");
                nanana = nanana.Replace(".db", "");
                comboBox_folder.Items.Add(nanana);
            }
        }

        public void thefolders() 
        {
            String path1 = Properties.Settings.Default.mainfolder + @"\" + "Projects";
            String path2 = Properties.Settings.Default.mainfolder + @"\" + "Backup";
            bool exists = System.IO.Directory.Exists(path1);
            if (!exists) { System.IO.Directory.CreateDirectory(path1); }
            exists = System.IO.Directory.Exists(path2);
            if (!exists) { System.IO.Directory.CreateDirectory(path2); }
        }

        public void autosave() 
        {
            //autosave
            if (Properties.Settings.Default.minutes_sv > 0)
            {
                timer_save.Interval = Properties.Settings.Default.minutes_sv * 60000;
            }
            if (Properties.Settings.Default.autosv == true) { timer_save.Enabled = true; }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            config f3 = new config();
            f3.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            
            
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

            newfolder = Microsoft.VisualBasic.Interaction.InputBox("A folder will be created with this name.", "Name the project...", "Default", -1, -1);
            String path = Properties.Settings.Default.mainfolder + @"\" + "Projects" + @"\" + newfolder;
            bool exists = System.IO.Directory.Exists(path);
            if (!exists) { System.IO.Directory.CreateDirectory(path); comboBox_folder.Items.Add(newfolder); }

        }

        public void checkpoint() 
        {
            thefolder = comboBox_folder.Text.ToString();
            String path = Properties.Settings.Default.mainfolder + @"\" + "Projects" + @"\" + thefolder;
            String pathbc = Properties.Settings.Default.mainfolder + @"\" + "Backup" + @"\" + thefolder;
            bool exists = System.IO.Directory.Exists(pathbc);
            if (!exists) { System.IO.Directory.CreateDirectory(pathbc); }

            backgroundWorker1.RunWorkerAsync();

            updatehdd();

        
        }

        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            String path = Properties.Settings.Default.mainfolder + @"\" + "Projects" + @"\" + thefolder;
            string[] files = System.IO.Directory.GetFiles(path, "*.db");
            String pathfrom = files[0];
            String pathto = Properties.Settings.Default.mainfolder + @"\" + "Backup" + @"\" + thefolder;
            String backname = files[0].Replace(path + @"\", "");
            backname = backname.Replace(".db", "");
            var dbfiles = Directory.EnumerateFiles(pathto, "*.db");
            int num = 0;
            foreach (string c in dbfiles) { num++; }
            num = num + 1;

            File.Copy(pathfrom,pathto + @"\" + backname + "_" +num.ToString() + ".db");
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            checkpoint();
        }

        public void updatehdd() 
        {
            var dbfiles = Directory.EnumerateFiles(Properties.Settings.Default.mainfolder + @"\" + "Backup", "*.db", SearchOption.AllDirectories);
            long val = 0;
            long val2 = 0;
            foreach (string c in dbfiles) 
            { 
                val2 = new System.IO.FileInfo(c).Length;
                val = val + val2; 
            }
            double gbtot = Math.Round((float)((val * 100 / 10737418240)));
            progressBar1.Value = (int)gbtot;
            gbtot = gbtot / 10;
            label1.Text = gbtot.ToString("n1") + " GB";
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            FormLoad f4 = new FormLoad();
            f4.Show();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            String selected_project = comboBox_folder.Text.ToString();
            //get original name
            String path = Properties.Settings.Default.mainfolder + @"\" + "Projects" + @"\" + selected_project;
            string[] files = System.IO.Directory.GetFiles(path, "*.db");
            String pathfrom = files[0];
            String backname = files[0].Replace(path + @"\", "");

            mensaje.Text = backname + " has been saved successfully...";
            timermessage.Enabled = true;
        }

        private void timermessage_Tick(object sender, EventArgs e)
        {
            mensaje.Text = "";
            timermessage.Enabled = false;
        }

        private void timer_save_Tick(object sender, EventArgs e)
        {
            checkpoint();
            timer_save.Interval = Properties.Settings.Default.minutes_sv * 60000;

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", Properties.Settings.Default.mainfolder);
        }

        int count = 0;
        private void timersplashscreen_Tick_1(object sender, EventArgs e)
        {
            count++;
            if (count >= 3)
            {

                timersplashscreen.Enabled = false;
                this.Opacity = 1;

            }
        }
        
    }
}
