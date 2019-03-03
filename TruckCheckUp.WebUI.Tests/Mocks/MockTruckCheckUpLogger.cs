using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Contracts.Logger;

namespace TruckCheckUp.WebUI.Tests.Mocks
{
    public class MockTruckCheckUpLogger : ILogger
    {
        private readonly ILog _log;

        public MockTruckCheckUpLogger()
        {
            _log = LogManager.GetLogger(typeof(MockTruckCheckUpLogger));
        }

        public void Info(string message)
        {
            return;
        }

        public void Debug(string message)
        {
            return;
        }
        public void Warning(string message)
        {
            return;
        }

        public void Error(string message, Exception ex)
        {
            return;
        }

        public void Fatal(string message, Exception ex)
        {
            return;
        }
    }
}
