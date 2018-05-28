using Lunalipse.Utilities;
using Lunalipse.Common.Interfaces.II18N;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Core.I18N
{
    /// <summary>
    /// 适用于国际化的页面集合。
    /// </summary>
    public class I18NPages : II18NPages
    {
        static volatile I18NPages p_insatnce;
        static readonly object p_lock = new object();

        public static I18NPages INSTANCE
        {
            get
            {
                if (p_insatnce == null)
                {
                    lock (p_lock)
                    {
                        p_insatnce = p_insatnce ?? new I18NPages();
                    }
                }
                return p_insatnce;
            }
        }
        private I18NPages() {}
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
