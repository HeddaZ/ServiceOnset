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
        #region IDisposable

        private bool disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    #region Dispose managed resources

                    if (this.InnerProcess != null)
                    {
                        try
                        {
                            if (!this.InnerProcess.HasExited)
                            {
                                this.InnerProcess.Kill();
                            }
                            this.InnerProcess.Dispose();
                        }
                        catch
                        {
                        }
                        finally
                        {
                            this.InnerProcess = null;
                        }
                    }

                    #endregion
                }

                #region Clean up unmanaged resources

                //

                #endregion

                this.disposed = true;
            }

            base.Dispose(disposing);
        }

        ~DaemonService()
        {
            this.Dispose(false);
        }

        #endregion

        protected Process InnerProcess
        {
            get;
            private set;
        }

        public DaemonService(IServiceStartInfo startInfo)
            : base(startInfo)
        {
            this.InnerProcess = new Process();
            this.InnerProcess.StartInfo.ErrorDialog = false;
            this.InnerProcess.StartInfo.CreateNoWindow = true;
            this.InnerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            this.Log.Info("InnerProcess is created with hidden UI");

            this.InnerProcess.StartInfo.UseShellExecute = false;
            this.InnerProcess.StartInfo.RedirectStandardError = true;
            this.InnerProcess.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                this.Log.Error("InnerProcess error: " + e.Data);
            });
            this.InnerProcess.StartInfo.RedirectStandardOutput = true;
            this.InnerProcess.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                this.Log.Error("InnerProcess output: " + e.Data);
            }); ;

            this.InnerProcess.StartInfo.FileName = startInfo.Command;
            this.InnerProcess.StartInfo.Arguments = startInfo.Arguments;
            this.InnerProcess.StartInfo.WorkingDirectory = startInfo.WorkingDirectory;
        }

        protected override void ThreadProc()
        {
            if (this.IsRunning)
            {
                this.InnerProcess.Start();

                if (this.InnerProcess.StartInfo.RedirectStandardOutput)
                {
                    this.InnerProcess.BeginOutputReadLine();
                }
                if (this.InnerProcess.StartInfo.RedirectStandardError)
                {
                    this.InnerProcess.BeginErrorReadLine();
                }

                this.InnerProcess.WaitForExit();
            }
        }
    }
}
