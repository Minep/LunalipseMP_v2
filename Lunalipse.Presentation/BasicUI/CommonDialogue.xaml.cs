using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lunalipse.Utilities;

namespace Lunalipse.Presentation.BasicUI
{
    /// <summary>
    /// CommonDialogue.xaml 的交互逻辑
    /// </summary>
    public partial class CommonDialogue : Page
    {
        public CommonDialogue()
        {
            InitializeComponent();
        }

        public CommonDialogue(string context, bool YesNo, Action<bool> btnPress) : this()
        {
            cnt.Text = context;
            if (!YesNo) no.Visibility = Visibility.Hidden;
            if(btnPress!=null)
            {
                yes.Click += delegate { btnPress?.Invoke(true); };
                no.Click += delegate { btnPress?.Invoke(false); };
            }
            else
            {
                yes.Click += __pressDefault;
                no.Click += __pressDefault;
            }
        }

        public CommonDialogue(string context, bool YesNo)
        {
            cnt.Text = context;
            if (!YesNo) no.Visibility = Visibility.Hidden;
            yes.Click += __pressDefault;
            no.Click += __pressDefault;
        }


        private void __pressDefault(object sender, EventArgs ea)
        {
            this.GetAncestor<Window>()?.Close();
        }
    }
}
