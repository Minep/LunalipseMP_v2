using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.IConsole
{
    public class ConsoleOut
    {
        public delegate void CommandWritOutLine(string output, params object[] args);
        public delegate void CommandWritOut(string output, params object[] args);
        public static CommandWritOutLine onWriteln;
        public static CommandWritOut onWrite;
        public static void WriteLine(string output, params object[] args)
        {
            onWriteln?.Invoke(output, args);
        }
        public static void Write(string output, params object[] args)
        {
            onWrite?.Invoke(output, args);
        }
    }
}
