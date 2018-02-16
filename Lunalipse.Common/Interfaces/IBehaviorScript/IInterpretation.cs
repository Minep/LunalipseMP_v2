using Lunalipse.Common.Data.BehaviorScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.IBehaviorScript
{
    public interface IInterpretation
    {
        List<ActionToken> Interpret(List<ScriptToken> tokens);
        bool AddParameterPattern(int type, string[] pattern);
        bool RemoveParameterPattern(int type);
        string[] GetPattern(int type);
    }
}
