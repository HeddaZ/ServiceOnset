using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceOnset
{
    public class Logger
    {
        private const string DefaultLoggerName = "Default";

        public ILog InnerLogger
        {
            get;
            private set;
        }
        public bool Enabled
        {
            get;
            set;
        }

        internal Logger(string name, bool enabled)
        {
            this.InnerLogger = LogManager.GetLogger(name);
            this.Enabled = enabled && AppHelper.Config.EnableLog;
        }
        internal Logger(bool enabled)
            : this(DefaultLoggerName, enabled)
        {
        }

        #region Logging

        public void Info(string format, params object[] args)
        {
            if (this.Enabled)
            {
                this.InnerLogger.InfoFormat(format, args);
            }
        }
        public void Info(string message)
        {
            if (this.Enabled)
            {
                this.InnerLogger.Info(message);
            }
        }
        public void Info(string message, Exception exception)
        {
            if (this.Enabled)
            {
                this.InnerLogger.Info(message, exception);
            }
        }

        public void Error(string format, params object[] args)
        {
            if (this.Enabled)
            {
                this.InnerLogger.ErrorFormat(format, args);
            }
        }
        public void Error(string message)
        {
            if (this.Enabled)
            {
                this.InnerLogger.Error(message);
            }
        }
        public void Error(string message, Exception exception)
        {
            if (this.Enabled)
            {
                this.InnerLogger.Error(message, exception);
            }
        }

        #endregion
    }
}
