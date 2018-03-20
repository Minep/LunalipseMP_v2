using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Lunalipse.Presentation.LpsWindow
{
    public class LunalipseDialogue : Window
    {
        public LunalipseDialogue()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Style = (Style)Application.Current.Resources["LunalipseDialogue"];
            Loaded += DialogueLoaded;
        }

        protected virtual void DialogueLoaded(object sender, EventArgs args)
        {
            ControlTemplate ct = (ControlTemplate)Application.Current.Resources["LunalipseDialogueBaseTemplate"];
            (ct.FindName("TitleBar", this) as Border).MouseDown += TitleBarMove;
            (ct.FindName("BtnClose", this) as Ellipse).MouseDown += ClosePressed;
        }

        protected void TitleBarMove(object sender, EventArgs args)
        {
            this.DragMove();
        }

        protected void ClosePressed(object sender, EventArgs args)
        {
            this.Close();
        }
    }
}
