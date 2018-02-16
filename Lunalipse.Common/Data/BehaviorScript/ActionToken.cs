using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Data.BehaviorScript
{
    public class ActionToken
    {
        public int Position;
        public int CommandType;
        public object[] ct_args;
        public int SuffixType;
        public object[] st_args;
    }
}
