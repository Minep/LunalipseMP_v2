using Lunalipse.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Lunalipse.Common.Generic.Cache.CacheInfo;

namespace Lunalipse.Core.Cache
{
    public class CacheUtils
    {
        public static string GenerateName(WinterWrapUp cw)
        {
            return "cch_{3}_{0}_{1}{2}".FormateEx(cw.deletable ? "t" : "f", cw.uid, CACHE_FILE_EXT, cw.markName);
        }

        public static WinterWrapUp ConvertToWWU(string name)
        {
            string[] sequence = name.Split('_');
            return new WinterWrapUp()
            {
                markName = sequence[1],
                deletable = sequence[2] == "t" ? true : false,
                uid = sequence[3]
            };
        }

        public static string GenerateMarkName(string type,params string[] additions)
        {
            string name = type;
            foreach(string s in additions)
            {
                name += "@{0}".FormateEx(s);
            }
            return name;
        }

        public static string CleanFormat(string ins)
        {
            String pattern = @"[\r|\n|\t]";
            String replaceValue = String.Empty;
            return Regex.Replace(ins, pattern, replaceValue);
        }
    }
}
