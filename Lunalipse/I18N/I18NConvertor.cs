using Lunalipse.Common.Interfaces.II18N;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.I18N
{
    public class I18NConvertor : II18NConvertor
    {
        private static I18NConvertor _cInstance = null;
        private static readonly object cILock = new object();

        public static I18NConvertor INSTANCE(II18NPages pgInstance = null)
        {
            if (_cInstance == null)
            {
                lock (cILock)
                {
                    _cInstance = _cInstance ?? new I18NConvertor(pgInstance);
                }
            }
            return _cInstance;
        }


        II18NPages Pages = null;

        I18NConvertor(II18NPages pgI)
        {
            if (pgI != null) Pages = pgI;
        }

        public string ConvertTo(string page, string key)
        {
            if (Pages == null) return key;
            return Pages.GetPage(page).getContext(key);
        }
    }
}
