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
            this.InnerProcess = CreateProcess();
        }

        protected override void ThreadProc()
        {
            if (this.IsRunning)
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
