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
        int Cmd2DefinedCmd(string cmd);
        int Suf2DefinedSuf(string suf);
        string DeifnedCmd2Cmd(uint cmd);
        string DeifnedSuf2Suf(uint suf);
        bool AddPrefix(uint type, string name);
        bool AddSuffix(uint type, string name);
        bool RemovePrefix(int type);
        bool RemoveSuffix(int type);
    }
}
