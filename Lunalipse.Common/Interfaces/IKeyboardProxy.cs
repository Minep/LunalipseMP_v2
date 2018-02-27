using Lunalipse.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces
{
    public interface IKeyboardProxy
    {
        bool RegistKeyEvent(KeyEventProc proc);
        bool RemoveKeyEvent(KeyEventProc proc);
        bool RemoveKeyEvent(string name);
        KeyEventProc GetKeyEvent(string name);
        KeyEventProc GetKeyEvent(int index);
        bool ChangeShortCut(int index, int Key, int Modifier);
        bool ChangeShortCut(string name, int Key, int Modifier);
        List<KeyEventProc> GetEventsList();
    }
}
