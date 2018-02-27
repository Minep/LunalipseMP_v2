using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Data
{
    public class KeyEventProc
    {
        public string Name { get; set; }
        // e.g. A
        public int SubKey { get; set; }
        // e.g. Alt
        public int ModifierKey{get; set;}
        public bool WaitRelease { get; set; }
        public Action ProcInvoke_Down, ProcInvoke_Up;
    }
}
