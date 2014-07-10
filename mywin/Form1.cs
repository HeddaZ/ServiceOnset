using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mywin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string s = DateTime.Now.ToString();
            using (StreamWriter w = new StreamWriter(Directory.GetCurrentDirectory() + "\\mywin.txt", false))
            {
                w.WriteLine(s);
            }
            label1.Text = s;
        }
    }
}
