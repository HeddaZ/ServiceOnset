﻿using log4net;
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    #region Dispose managed resources

                    if (IsRunning)
                    {
                        IsRunning = false;
                    }
                    if (InnerThread != null)
                    {
                        try
                        {
                            if (InnerThread.IsAlive)
                            {
                                InnerThread.Abort();
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            InnerThread = null;
                        }
                    }
                    StartInfo = null;
                    Log = null;

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
            Dispose(false);
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
            StartInfo = startInfo;
            Log = new Logger(startInfo.Name, startInfo.EnableLog);

            InnerThread = new Thread(new ThreadStart(ThreadProc));
            InnerThread.IsBackground = true;
            IsRunning = false;
        }

        public virtual void Start()
        {
            IsRunning = true;
            InnerThread.Start();
            Log.Info("InnerThread is started");
        }
        public virtual void Stop()
        {
            IsRunning = false;
            Log.Info("InnerThread is signalled to stop");
        }
        protected abstract void ThreadProc();

        #region Process helper

        protected Process CreateProcess()
        {
            if (StartInfo.KillExistingProcess)
            {
                Log.Info("Try to kill existing process");
                Process.GetProcesses().Where(p => TryMatchProcess(p, StartInfo.Command))
                    .ToList()
                    .ForEach(p => TryKillProcess(p));
            }

            var process = new Process();
            process.StartInfo.UseShellExecute = StartInfo.UseShellExecute;
            process.StartInfo.FileName = StartInfo.Command;
            process.StartInfo.Arguments = StartInfo.Arguments;
            process.StartInfo.WorkingDirectory = StartInfo.WorkingDirectory;
            Log.Info("InnerProcess is created");

            if (!StartInfo.UseShellExecute)
            {
                process.StartInfo.RedirectStandardError = true;
                process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    Log.Error("InnerProcess error: " + e.Data);
                });
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    Log.Info("InnerProcess output: " + e.Data);
                });

                if (StartInfo.HideWindow)
                {
                    process.StartInfo.CreateNoWindow = true;
                }
            }
            else
            {
                if (StartInfo.HideWindow)
                {
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                }
            }

            if (string.IsNullOrEmpty(Path.GetExtension(process.StartInfo.FileName)))
            {
                process.StartInfo.FileName = process.StartInfo.FileName.TrimEnd('.') + ".exe";
            }
            if (!File.Exists(process.StartInfo.FileName) && Directory.Exists(process.StartInfo.WorkingDirectory))
            {
                process.StartInfo.FileName = Path.Combine(process.StartInfo.WorkingDirectory, Path.GetFileName(process.StartInfo.FileName));
            }

            return process;
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
