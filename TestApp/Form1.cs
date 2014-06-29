using ServiceOnset;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            ServiceManager m = new ServiceManager(AppHelper.Config);
            m.Run();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text += "\r\n" + ServiceOnset.AppHelper.AppPath;
            textBox1.Text += "\r\n" + ServiceOnset.AppHelper.AppVersion;

            
        }
    }
}
