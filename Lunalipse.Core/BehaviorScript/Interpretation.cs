using Lunalipse.Common.Data.BehaviorScript;
using Lunalipse.Common.Generic;
using Lunalipse.Common.Interfaces.IBehaviorScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Core.BehaviorScript
{
    public class Interpretation : IInterpretation
    {
        public Dictionary<int, string[]> ParameterPattern = new Dictionary<int, string[]>()
        {
            {0x0001,
                new string[]{"String","Int32"} },
            {0x0002,
                new string[]{"Int32"} },
            {0x0003,
                new string[]{"String","String","Int32"} },
            {0x0004,
                new string[]{"Int32", "Int32", "Int32", "Int32", "Int32", "Int32", "Int32", "Int32", "Int32", "Int32"} },
            {0x0005,
                null},
            {0x0006,
                null},
            {0x0007,
                new string[]{"String"} },
            {0x1001,
                new string[]{"Int32","Int32"} },
            {0x1002,
                new string[]{"Int32"} }
        };
        public List<ActionToken> Interpret(List<ScriptToken> tokens)
        {
            List<ActionToken> ats = new List<ActionToken>();
            foreach(ScriptToken token in tokens)
            {
                ats.Add(_interpret(token));
            }
            return ats;
        }

        private ActionToken _interpret(ScriptToken token)
        {
            ActionToken at = new ActionToken();
            at.CommandType = (int)ScriptUtil.Cmd2DefinedCmd(token.Command);
            at.ct_args = ConvertArgs(at.CommandType, ParameterPattern[at.CommandType], token.Args);
            at.SuffixType = (int)ScriptUtil.Suf2DefinedSuf(token.TailFix);
            at.st_args = ConvertArgs(at.SuffixType, ParameterPattern[at.SuffixType], token.TailArgs);
            if (at.ct_args == null || at.st_args == null)
            {
                return null;
            }
            return at;
        }

        private object[] ConvertArgs(int cmdType, string[] argsType, string[] srcArg)
        {
            List<object> args = new List<object>();
            for (int i = 0; i < srcArg.Length; i++)
            {
                if (i > argsType.Length - 1)
                {
                    ErrorDelegation.OnErrorRaisedBSI?.Invoke("CORE_LBS_NoSuchPara", string.Join(",", argsType), -1);
                    return null;
                }
                Type t = Type.GetType("System." + argsType[i]);
                try
                {
                    args.Add(Convert.ChangeType(srcArg[i], t));
                }
                catch (FormatException)
                {
                    ErrorDelegation.OnErrorRaisedBSI?.Invoke("CORE_LBS_NoSuchPara", string.Join(",", argsType), -1);
                    return null;
                }
                catch (OverflowException)
                {
                    ErrorDelegation.OnErrorRaisedBSI?.Invoke("CORE_LBS_DataTypeOverflow", argsType[i], -1);
                    return null;
                }
            }
            return args.ToArray();
        }

        public bool AddParameterPattern(int type, string[] pattern)
        {
            return ParameterPattern.Add4nRep(type, pattern);
        }

        public bool RemoveParameterPattern(int type)
        {
            return ParameterPattern.Remove(type);
        }

        public string[] GetPattern(int type)
        {
            return ParameterPattern[type];
        }
    }
}
