using log4net;
using ServiceOnset.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServiceOnset.Services
{
    public abstract class ServiceBase : IService, IDisposable
    {
        #region IDisposable

        private bool disposed = false;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    #region Dispose managed resources

                    if (this.IsRunning)
                    {
                        this.IsRunning = false;
                    }
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
                    if (this.InnerThread != null)
                    {
                        try
                        {
                            if (this.InnerThread.IsAlive)
                            {
                                this.InnerThread.Abort();
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            this.InnerThread = null;
                        }
                    }

                    #endregion
                }

                #region Clean up unmanaged resources

                //

                #endregion

                disposed = true;
            }
        }

        ~ServiceBase()
        {
            this.Dispose(false);
        }

        #endregion

        protected ILog InnerLog
        {
            get;
            private set;
        }

        public IServiceStartInfo StartInfo
        {
            get;
            private set;
        }
        public Process InnerProcess
        {
            get;
            private set;
        }
        protected Thread InnerThread
        {
            get;
            private set;
        }
        protected bool IsRunning
        {
            get;
            set;
        }
        

        public ServiceBase(IServiceStartInfo startInfo)
        {
            this.InnerLog = LogManager.GetLogger(startInfo.);
            this.StartInfo = startInfo;
            this.InnerProcess = new Process();
            this.InnerThread = new Thread(() => this.ThreadProc(this.InnerProcess, this.StartInfo));
            this.InnerThread.IsBackground = true;
            this.IsRunning = true;
        }

        public virtual void Start()
        {
            this.InnerThread.Start();
        }
        protected abstract void ThreadProc(Process process, IServiceStartInfo startInfo);
    }
}
