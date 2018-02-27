using Lunalipse.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.IBehaviorScript
{
    public interface IInterpreter
    {
        bool Load(string ScriptID);
        bool LoadPath(string ScriptPath);
        bool SaveAs(string path);
        MusicEntity Stepping();

    }
}
