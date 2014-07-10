using ServiceOnset.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ServiceOnset.Services
{
    public class LaunchService : ServiceBase
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

        ~LaunchService()
        {
            this.Dispose(false);
        }

        #endregion

        protected Process InnerProcess
        {
            get;
            private set;
        }

        public LaunchService(IServiceStartInfo startInfo)
            : base(startInfo)
        {
            this.InnerProcess = new Process();
            this.InnerProcess.StartInfo.UseShellExecute = startInfo.UseShellExecute;

            this.InnerProcess.StartInfo.FileName = startInfo.Command;
            this.InnerProcess.StartInfo.Arguments = startInfo.Arguments;
            this.InnerProcess.StartInfo.WorkingDirectory = startInfo.WorkingDirectory;
            this.Log.Info("InnerProcess is created");

            this.ResolveProcessBeforeStart(this.InnerProcess);
        }

        protected override void ThreadProc()
        {
            if (this.IsRunning)
            {
                try
                {
                    this.InnerProcess.Start();
                    this.ResolveProcessAfterStart(this.InnerProcess);

                    //this.InnerProcess.WaitForExit();
                }
                catch (Exception exception)
                {
                    this.Log.Error("ThreadProc error --->", exception);
                }
            }
        }

        public override void Stop()
        {
            //if (!this.InnerProcess.HasExited)
            //{
                this.InnerProcess.Kill();
            //}
            base.Stop();
        }
    }
}
