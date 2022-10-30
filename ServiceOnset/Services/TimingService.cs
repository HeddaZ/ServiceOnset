using ServiceOnset.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceOnset.Services
{
    public class TimingService : ServiceBase
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

        ~TimingService()
        {
            Dispose(false);
        }

        #endregion

        protected Process InnerProcess
        {
            get;
            private set;
        }

        public TimingService(IServiceStartInfo startInfo)
            : base(startInfo)
        {
            InnerProcess = CreateProcess();
        }

        protected override void ThreadProc()
        {
            const string TimingExpFormat = "MMddHHmm";

            var timingExp = StartInfo.TimingExp; // 00
            var nowExpFormat = TimingExpFormat.Substring(Math.Max(0, TimingExpFormat.Length - timingExp.Length), Math.Min(TimingExpFormat.Length, timingExp.Length));

            while (IsRunning)
            {
                #region Timing strategy

                var nowExp = DateTime.Now.ToString(nowExpFormat);
                if (nowExp != timingExp)
                {
                    Thread.Sleep(60000);
                    continue;
                }

                #endregion

                try
                {
                    InnerProcess.Start();
                    EnableOutputRedirection(InnerProcess);

                    Thread.Sleep(60000);
                }
                catch (Exception exception)
                {
                    Log.Error("ThreadProc error --->", exception);

                    Thread.Sleep(60000);
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
