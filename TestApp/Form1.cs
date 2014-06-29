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

            var config = AppHelper.Config;

            var log = AppHelper.Log;

            for (int i = 0; i < 10;i++ )
                log.Info("wwwwwwww, heeda"+i.ToString());
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text += "\r\n" + ServiceOnset.AppHelper.AppPath;
            textBox1.Text += "\r\n" + ServiceOnset.AppHelper.AppVersion;

            
        }
    }
}
