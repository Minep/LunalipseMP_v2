using Lunalipse.Common.Data.BehaviorScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.IBehaviorScript
{
    public interface IParser
    {
        bool Load(string id, bool append);
        bool LoadPath(string path, bool append);
        bool Validator(string line);
        bool Parse();
        ScriptToken Tokenize(string l);
    }
}
