using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Data.BehaviorScript
{
    public struct ScriptToken
    {
        public string Prefix;
        public string Command;
        public string[] Args;
        public string TailFix;
        public string[] TailArgs;
    }
}
