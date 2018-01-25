using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Win32
{
    public enum WM : int
    {
        WM_KEYDOWN = 0x100,     //KEYDOWN
        WM_KEYUP = 0x101,       //KEYUP
        WM_SYSKEYDOWN = 0x104,  //SYSKEYDOWN
        WM_SYSKEYUP = 0x105     //SYSKEYUP
}
}
