using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Infrastructure.Utilities
{
    public static class LogHelper
    {
        /// <summary>
        /// Write log to debug , See output window for detail
        /// </summary>
        /// <param name="TAG"></param>
        /// <param name="e"></param>
        public static void WriteLog(string TAG,Exception e)
        {
            Debug.WriteLine(TAG +"=>" + e.Message);
        }

        /// <summary>
        /// Write log to debug , See output window for detail
        /// </summary>
        /// <param name="TAG"></param>
        /// <param name="Message">string Message</param>
        public static void WriteLog(string TAG, string Message)
        {
            Debug.WriteLine(TAG + "=>" +Message);
        }
    }
}
