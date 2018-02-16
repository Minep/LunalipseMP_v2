using Lunalipse.Common.Data.BehaviorScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Core.BehaviorScript
{
    internal class ScriptUtil
    {
        public static DefinedCmd Cmd2DefinedCmd(string cmd)
        {
            if (cmd.StartsWith("luna.", true, null))
            {
                cmd = cmd.Replace('.', '_');
            }
            else
            {
                string _c = cmd;
                cmd = "LUNA_" + _c;
            }
            cmd = cmd.ToUpper();
            return Enum.IsDefined(typeof(DefinedCmd), cmd) ? (DefinedCmd)Enum.Parse(typeof(DefinedCmd), cmd) : DefinedCmd.CMD_NAN;
        }

        public static DefinedSuffix Suf2DefinedSuf(string suf)
        {
            if (suf.StartsWith("sufx.", true, null))
            {
                suf = suf.Replace('.', '_');
            }
            else
            {
                string _c = suf;
                suf = "SUFX_" + _c;
            }
            suf = suf.ToUpper();
            return Enum.IsDefined(typeof(DefinedSuffix), suf) ? (DefinedSuffix)Enum.Parse(typeof(DefinedSuffix), suf) : DefinedSuffix.SUFX_NAN;
        }

        public static string DeifnedCmd2Cmd(uint cmd)
        {
            return Enum.GetName(typeof(DefinedCmd), cmd);
        }
        public static string DeifnedSuf2Suf(uint suf)
        {
            return Enum.GetName(typeof(DefinedSuffix), suf);
        }

        public static string SanitaizeParameter(string para)
        {
            if (para.EndsWith("\""))
            {
                para = para.Remove(para.Length - 1, 1);
            }
            if (para.StartsWith("\""))
            {
                para = para.Remove(0, 1);
            }
            return para;
        }
    }
}
