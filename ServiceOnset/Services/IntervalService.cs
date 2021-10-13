using ServiceOnset.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServiceOnset.Services
{
    public class IntervalService : ServiceBase
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

        ~IntervalService()
        {
            Dispose(false);
        }

        #endregion

        protected Process InnerProcess
        {
            get;
            private set;
        }

        public IntervalService(IServiceStartInfo startInfo)
            : base(startInfo)
        {
            InnerProcess = CreateProcess();
        }

        protected override void ThreadProc()
        {
            while (IsRunning)
            {
                try
                {
                    InnerProcess.Start();
                    EnableOutputRedirection(InnerProcess);

                    Thread.Sleep(StartInfo.IntervalInSeconds * 1000);
                }
                catch (Exception exception)
                {
                    Log.Error("ThreadProc error --->", exception);

                    Thread.Sleep(StartInfo.IntervalInSeconds * 1000);
                }
                finally
                {
                    DisableOutputRedirection(InnerProcess);
                    try
                    {
                        InnerProcess.Kill();
                    }
                    catch { }
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
