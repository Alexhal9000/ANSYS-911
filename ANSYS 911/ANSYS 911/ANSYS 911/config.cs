using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ANSYS_911
{
    public partial class config : Form
    {
        float maxgb;

        public config()
        {
            InitializeComponent();
        }

        public void trackBar1_Scroll(object sender, EventArgs e)
        {
            maxgb = (float)trackBar1.Value / 10;

            lab_maxgb.Text = maxgb.ToString("n1") + " GB";
        }

        private void config_Load(object sender, EventArgs e)
        {
            trackBar1.Value = (int)(Properties.Settings.Default.maxhdd)*10;
            lab_maxgb.Text = Properties.Settings.Default.maxhdd.ToString("n1") + " GB";
            textBox_mom.Text = Properties.Settings.Default.mainfolder;

            if (Properties.Settings.Default.autosv == true)
            {
                checkBox_autosv.Checked = true;
                numeric_minsv.Enabled = true;
                numeric_minsv.Value = Properties.Settings.Default.minutes_sv;
            }
            else {
                checkBox_autosv.Checked = false;
                numeric_minsv.Enabled = false;
            }
        }

        private void checkBox_autosv_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_autosv.Checked == true) {
                numeric_minsv.Enabled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox_mom.Text = folderBrowserDialog1.SelectedPath.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static Form1 f1 = new Form1();
        public void button2_Click(object sender, EventArgs e)
        {
            maxgb = (float)trackBar1.Value / 10;
            Properties.Settings.Default.maxhdd = maxgb;
            Properties.Settings.Default.autosv = checkBox_autosv.Checked;
            Properties.Settings.Default.minutes_sv = (int)numeric_minsv.Value;
            Properties.Settings.Default.mainfolder = textBox_mom.Text;
            Properties.Settings.Default.Save();

            Form1 ff = new Form1();
            ff.thefolders();
            ff.updatehdd();
            ff.getcombobox();
            ff.autosave();

            this.Close();
        }

        
    }
}
