using ServiceOnset.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ServiceOnset.Services
{
    public class DaemonService : ServiceBase
    {
        public DaemonService(IServiceStartInfo startInfo)
            : base(startInfo)
        {
        }

        public override void Start()
        {
            this.InnerProcess.StartInfo.FileName = this.StartInfo.Command;
            this.InnerProcess.StartInfo.Arguments = this.StartInfo.Arguments;
            this.InnerProcess.StartInfo.WorkingDirectory = this.StartInfo.InitialDirectory;

            this.InnerProcess.StartInfo.UseShellExecute = false;
            this.InnerProcess.StartInfo.ErrorDialog = false;
            this.InnerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            this.InnerProcess.StartInfo.RedirectStandardOutput = true;

            this.InnerProcess.Start();
        }
    }
}
