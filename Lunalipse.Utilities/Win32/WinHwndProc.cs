using System;
namespace Lunalipse.Utilities.Win32
{
    public abstract class WinHwndProc
    {
        public virtual IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            return IntPtr.Zero;
        }
    }
}
