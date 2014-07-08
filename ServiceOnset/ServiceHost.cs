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
        public ServiceHost()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ServiceManager m = new ServiceManager(AppHelper.Config);
            m.RunServices();
        }

        protected override void OnStop()
        {
        }
    }
}
