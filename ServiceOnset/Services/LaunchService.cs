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
            if (!disposed)
            {
                if (disposing)
                {
                    #region Dispose managed resources

                    if (InnerProcess != null)
                    {
                        try
                        {
                            InnerProcess.Dispose();
                        }
                        catch
                        {
                        }
                        finally
                        {
                            InnerProcess = null;
                        }
                    }

                    #endregion
                }

                #region Clean up unmanaged resources

                //

                #endregion

                disposed = true;
            }

            base.Dispose(disposing);
        }

        ~LaunchService()
        {
            Dispose(false);
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
            InnerProcess = CreateProcess();
        }

        protected override void ThreadProc()
        {
            if (IsRunning)
            {
                try
                {
                    InnerProcess.Start();
                    EnableOutputRedirection(InnerProcess);

                    InnerProcess.WaitForExit();
                }
                catch (Exception exception)
                {
                    Log.Error("ThreadProc error --->", exception);

                    try
                    {
                        InnerProcess.Kill();
                    }
                    catch { }
                }
                finally
                {
                    DisableOutputRedirection(InnerProcess);
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
                InnerProcess.Kill();
            }
            catch { }
        }
    }
}
