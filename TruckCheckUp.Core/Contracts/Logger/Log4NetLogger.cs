using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Contracts.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger()
        {
            _log = LogManager.GetLogger(typeof(Log4NetLogger));
        }

        public void Info(string message)
        {
            _log.Info(message);
        }

        public void Debug(string message)
        {
            _log.Debug(message);
        }
        public void Warning(string message)
        {
            _log.Warn(message);
        }

        public void Error(string message, Exception ex)
        {
            _log.Error(message, ex);
        }

        public void Fatal(string message, Exception ex)
        {
            _log.Fatal(message, ex);
        }
    }
}
