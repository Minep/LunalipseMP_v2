using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Core.GlobalSetting
{
    public class GSettingDelegation
    {
        public delegate void XmlFormatUncorrect(string message);
        public delegate void XmlConfigVersionUnmacth(string storedVeriosn, string curVersion);

        public static XmlFormatUncorrect OnFormatUncorrect;
        public static XmlConfigVersionUnmacth OnVersionUnmatch;
    }
}
