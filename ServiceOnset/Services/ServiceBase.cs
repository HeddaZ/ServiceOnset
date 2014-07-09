using log4net;
using ServiceOnset.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServiceOnset.Services
{
    public abstract class ServiceBase : IService
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
                    this.StartInfo = null;
                    this.Log = null;

                    #endregion
                }

                #region Clean up unmanaged resources

                //

                #endregion

                this.disposed = true;
            }
        }

        ~ServiceBase()
        {
            this.Dispose(false);
        }

        #endregion

        public IServiceStartInfo StartInfo
        {
            get;
            private set;
        }
        protected Logger Log
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
            this.StartInfo = startInfo;
            this.Log = new Logger(startInfo.Name, startInfo.EnableLog);

            this.InnerThread = new Thread(new ThreadStart(this.ThreadProc));
            this.InnerThread.IsBackground = true;

            this.IsRunning = false;
        }

        public virtual void Start()
        {
            this.IsRunning = true;
            this.InnerThread.Start();
            this.Log.Info("InnerThread is started");
        }
        public virtual void Stop()
        {
            this.IsRunning = false;
            this.Log.Info("InnerThread is signalled to stop");
        }
        protected abstract void ThreadProc();
    }
}
