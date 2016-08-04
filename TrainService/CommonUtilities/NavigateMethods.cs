using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities
{
    internal static class NativeMethods
    {
        /// <summary>
        /// This method used to reset system error mode.
        /// Please see:<see cref="http://stackoverflow.com/questions/673036/how-to-handle-a-crash-in-a-process-launched-via-system-diagnostics-process"/>
        /// </summary>
        /// <param name="wMode"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int SetErrorMode(int wMode);
    }
}
