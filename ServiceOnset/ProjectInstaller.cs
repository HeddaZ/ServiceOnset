using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Management;

namespace ServiceOnset
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller1_Committed(object sender, InstallEventArgs e)
        {
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = ImpersonationLevel.Impersonate;
            ManagementScope scope = new ManagementScope(@"root\CIMV2", options);
            scope.Connect();

            ManagementObject wmi = new ManagementObject("Win32_Service.Name='" + serviceInstaller1.ServiceName + "'");
            ManagementBaseObject inParam = wmi.GetMethodParameters("Change");
            inParam["DesktopInteract"] = true;
            ManagementBaseObject outParam = wmi.InvokeMethod("Change", inParam, null);
        }
    }
}
