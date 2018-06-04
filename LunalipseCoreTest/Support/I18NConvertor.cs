using Lunalipse.Common.Interfaces.II18N;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunalipseCoreTest.Support
{
    public class I18NConvertor : II18NConvertor
    {
        public string ConvertTo(string page, string key)
        {
            return key;
        }
    }
}
