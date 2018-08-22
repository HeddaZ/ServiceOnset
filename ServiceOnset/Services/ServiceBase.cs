using log4net;
using ServiceOnset.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            if (startInfo.KillExistingProcess)
            {
                Process.GetProcesses().Where(p => TryMatchProcess(p, startInfo.Command))
                    .ToList()
                    .ForEach(p => TryKillProcess(p));
            }
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

        #region Process helper

        protected void ResolveProcessBeforeStart(Process process)
        {
            if (!process.StartInfo.UseShellExecute)
            {
                process.StartInfo.RedirectStandardError = true;
                process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    this.Log.Error("InnerProcess error: " + e.Data);
                });
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    this.Log.Info("InnerProcess output: " + e.Data);
                });

            }
            else
            {

            }

            if (string.IsNullOrEmpty(Path.GetExtension(process.StartInfo.FileName)))
            {
                process.StartInfo.FileName = process.StartInfo.FileName.TrimEnd('.') + ".exe";
            }
            if (!File.Exists(process.StartInfo.FileName) && Directory.Exists(process.StartInfo.WorkingDirectory))
            {
                process.StartInfo.FileName = Path.Combine(process.StartInfo.WorkingDirectory, Path.GetFileName(process.StartInfo.FileName));
            }
        }

        protected static void EnableOutputRedirection(Process process)
        {
            try
            {
                if (process.StartInfo.RedirectStandardOutput)
                {
                    process.BeginOutputReadLine();
                }
                if (process.StartInfo.RedirectStandardError)
                {
                    process.BeginErrorReadLine();
                }
            }
            catch { }
        }
        protected static void DisableOutputRedirection(Process process)
        {
            try
            {
                if (process.StartInfo.RedirectStandardError)
                {
                    process.CancelErrorRead();
                }
                if (process.StartInfo.RedirectStandardOutput)
                {
                    process.CancelOutputRead();
                }
            }
            catch { }
        }
        protected static bool TryMatchProcess(Process process, string command)
        {
            try
            {
                return process.MainModule != null
                    && process.MainModule.FileName != null
                    && (Path.GetFileName(process.MainModule.FileName).Equals(Path.GetFileName(command), StringComparison.OrdinalIgnoreCase)
                        || Path.GetFileNameWithoutExtension(process.MainModule.FileName).Equals(Path.GetFileNameWithoutExtension(command), StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return false;
            }
        }
        protected static void TryKillProcess(Process process)
        {
            try
            {
                process.Kill();
            }
            catch
            {
                const string taskkillCommand = "TASKKILL";
                const string taskkillArguments = "/PID {0}";
                const string ntsdCommand = "ntsd";
                const string ntsdArguments = "-c q -p {0}";
                try
                {
                    using (Process killer = new Process())
                    {
                        if (System.Environment.OSVersion.Version.Major >= 6)
                        {
                            killer.StartInfo.FileName = taskkillCommand;
                            killer.StartInfo.Arguments = string.Format(taskkillArguments, process.Id);
                        }
                        else
                        {
                            killer.StartInfo.FileName = ntsdCommand;
                            killer.StartInfo.Arguments = string.Format(ntsdArguments, process.Id);
                        }
                        killer.StartInfo.UseShellExecute = true;
                        killer.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        killer.Start();
                        killer.WaitForExit();
                        killer.Close();
                    }
                }
                catch { }
            }
        }

        #endregion
    }
}
