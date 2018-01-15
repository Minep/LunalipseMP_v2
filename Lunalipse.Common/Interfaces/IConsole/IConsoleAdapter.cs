
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.IConsole
{
    public interface IConsoleAdapter
    {
        bool RegisterComponent(string component, ComponentHandler CH);
        bool UnregisterComponent(string component);
        ComponentHandler getComponent(string component);
        bool InvokeCommand(string cmd, params string[] args);
    }
}
