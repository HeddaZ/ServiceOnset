using ServiceOnset.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServiceOnset.Services
{
    public class DaemonService : ServiceBase
    {
        public DaemonService(IServiceStartInfo startInfo)
            : base(startInfo)
        {
        }

        protected override void ThreadProc()
        {
            if (this.IsRunning)
            {
                this.InnerProcess.StartInfo.FileName = this.StartInfo.Command;
                this.InnerProcess.StartInfo.Arguments = this.StartInfo.Arguments;
                this.InnerProcess.StartInfo.WorkingDirectory = this.StartInfo.InitialDirectory;

                this.InnerProcess.Start();
                this.InnerLogger.InfoFormat("Process [{0}] started", this.StartInfo.Name);

                if (this.InnerProcess.StartInfo.RedirectStandardOutput)
                {
                    this.InnerProcess.BeginOutputReadLine();
                }
                if (this.InnerProcess.StartInfo.RedirectStandardError)
                {
                    this.InnerProcess.BeginErrorReadLine();
                }

                this.InnerProcess.WaitForExit();
                this.InnerLogger.InfoFormat("Process [{0}] exited", this.StartInfo.Name);
            }
        }
    }
}
