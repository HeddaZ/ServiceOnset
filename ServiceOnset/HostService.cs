using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ServiceOnset
{
    public partial class HostService : ServiceBase
    {
        public HostService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Process.Start(Process.Start())
        }

        protected override void OnStop()
        {
        }
    }
}
