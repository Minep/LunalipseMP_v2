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
using Lunalipse.Common;
using Lunalipse.Core.PlayList;
using Lunalipse.Core.Metadata;
using Lunalipse.Core.LpsAudio;
using Lunalipse.Common.Data;
using Lunalipse.Core.BehaviorScript;

namespace Lunalipse
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MusicListPool mlp;
        MediaMetaDataReader mmdr;
        LpsAudio laudio;
        Interpreter intp;
        public MainWindow()
        {
            InitializeComponent();
            mlp = MusicListPool.INSATNCE;
            laudio = LpsAudio.INSTANCE();
            mmdr = new MediaMetaDataReader();
            mlp.AddToPool("F:/M2", mmdr);
            dipMusic.ItemsSource = mlp.Musics;
            intp = Interpreter.INSTANCE(@"F:\Lunalipse\TestUnit\bin\Debug");
            if (intp.Load("prg2"))
            {
                PlayFinished();
            }
            alb.Source = mlp.ToCatalogue().GetCatalogueCover();

            AudioDelegations.PostionChanged += (x) =>
            {
                Console.WriteLine(x);
            };
            AudioDelegations.PlayingFinished += PlayFinished;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.EnableBlur();
        }


        private void Window_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void dipMusic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

                laudio.Load((MusicEntity)dipMusic.SelectedItem);
                laudio.Play();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            laudio.Dispose();
        }

        private void PlayFinished()
        {
            MusicEntity MEnt;
            MEnt = intp.Stepping();
            if (MEnt != null)
            {
                laudio.Load(MEnt);
                laudio.Play();
            }
            else return;
        }
    }
}
