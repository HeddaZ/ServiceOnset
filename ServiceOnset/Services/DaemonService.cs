using ServiceOnset.Common;
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
        public DaemonService(IServiceStartInfo startInfo) :
            base(startInfo)
        {
        }

        protected override void ThreadProc(Process process, IServiceStartInfo startInfo)
        {
            process.StartInfo.FileName = startInfo.Command;
            process.StartInfo.Arguments = startInfo.Arguments;
            process.StartInfo.WorkingDirectory = startInfo.InitialDirectory;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.ErrorDialog = false;
            process.StartInfo.CreateNoWindow = true;

            process.

            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();
        }
    }
}
