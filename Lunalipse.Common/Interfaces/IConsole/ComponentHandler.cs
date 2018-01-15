using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.IConsole
{
    public abstract class ComponentHandler
    {       
        public virtual bool OnCommand(params string[] args)
        {
            return false;
        }
    }
}
