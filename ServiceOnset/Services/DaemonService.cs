using ServiceOnset.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ServiceOnset.Services
{
    public class DaemonService : ServiceBase
    {
        public DaemonService(IServiceStartInfo startInfo) :
            base(startInfo)
        {
        }

        public override void Start()
        {
            this.InternalProcess.StartInfo.FileName = this.StartInfo.Command;
            this.InternalProcess.StartInfo.Arguments = this.StartInfo.Arguments;
            this.InternalProcess.StartInfo.WorkingDirectory = this.StartInfo.InitialDirectory;
            this.InternalProcess.StartInfo.UseShellExecute = false;
            this.InternalProcess.StartInfo.ErrorDialog = false;
            this.InternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            this.InternalProcess.StartInfo.RedirectStandardOutput = true;

            this.InternalProcess.Start();
        }
    }
}
