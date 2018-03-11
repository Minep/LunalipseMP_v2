using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Utilities.Win32
{
    public enum DbtDevice : int
    {
        DbtDevicearrival = 0x8000,       
        DbtDeviceremovecomplete = 0x8004,    
        WmDevicechange = 0x0219    
    }
}
