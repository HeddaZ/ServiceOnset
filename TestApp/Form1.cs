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
        ServiceManager m;

        public Form1()
        {
            InitializeComponent();

            //Process[] ps = Process.GetProcesses();
            //var p1 = ps.Select(p => p.MainModule.ModuleName);
            //var p2 = p1.Where(p => p != null);
            var v = System.Environment.OSVersion.Version;
            m = new ServiceManager(AppHelper.Config);
            m.RunServices();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m.StopServices();
        }
    }
}
