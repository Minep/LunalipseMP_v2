using Lunalipse.Common.Generic;
using Lunalipse.Common.Interfaces.II18N;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Core.I18N
{
    public class I18NPage : II18NPage
    {
        private Dictionary<string, II18NCollection> Pages = new Dictionary<string, II18NCollection>();
        public bool AddPage(string name, II18NCollection pageCollection)
        {
            return Pages.Add4nRep(name, pageCollection);
        }

        public bool DropPage(string name)
        {
            return Pages.Remove(name);
        }

        public II18NCollection GetPage(string pageName)
        {
            if (Pages.ContainsKey(pageName)) return Pages[pageName];
            return null;
        }
    }
}
