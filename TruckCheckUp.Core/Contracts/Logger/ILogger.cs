using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Contracts.Logger
{
    public interface ILogger
    {
        void Info(string message);
        void Debug(string message);
        void Warning(string message);
        void Error(string message, Exception ex);
        void Fatal(string message, Exception ex);
    }
}
