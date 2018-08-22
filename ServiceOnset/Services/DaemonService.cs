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
            this.InnerProcess = CreateProcess();
        }

        protected override void ThreadProc()
        {
            while (this.IsRunning)
            {
                try
                {
                    this.InnerProcess.Start();
                    EnableOutputRedirection(this.InnerProcess);

                    this.InnerProcess.WaitForExit();
                }
                catch (Exception exception)
                {
                    this.Log.Error("ThreadProc error --->", exception);

                    try
                    {
                        this.InnerProcess.Kill();
                    }
                    catch { }
                }
                finally
                {
                    DisableOutputRedirection(this.InnerProcess);
                }

                Thread.Sleep(this.StartInfo.IntervalInSeconds * 1000);
            }
        }

        public override void Start()
        {
            base.Start();
        }
        public override void Stop()
        {
            base.Stop();

            try
            {
                this.InnerProcess.Kill();
            }
            catch { }
        }
    }
}
