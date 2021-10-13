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
    public partial class ServiceHost : ServiceBase
    {
        private ServiceManager ServiceManager { get; set; }

        public ServiceHost()
        {
            InitializeComponent();
            ServiceManager = new ServiceManager(AppHelper.Config);
        }

        protected override void OnStart(string[] args)
        {
            ServiceManager.RunServices();
        }

        protected override void OnStop()
        {
            ServiceManager.StopServices();
        }
    }
}
