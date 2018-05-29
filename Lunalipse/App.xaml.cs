using Lunalipse.Common.Data;
using Lunalipse.Core.I18N;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Lunalipse
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            I18NTokenizer I18T = new I18NTokenizer();
            if(!I18T.LoadFromFile(@"Data\i18n.lang"))
            {
                Current.Shutdown();
            }
            I18T.GetPages(SupportLanguages.CHINESE_SIM);
        }
    }
}
