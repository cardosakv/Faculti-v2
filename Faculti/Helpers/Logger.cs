using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Faculti.Helpers
{
    public class Logger
    {
        public static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
    }
}
