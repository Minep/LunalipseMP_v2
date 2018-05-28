using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lunalipse.Core.PlayList;
using Lunalipse.Core.Metadata;
using Lunalipse.Core.LpsAudio;
using Lunalipse.Common.Data;
using Lunalipse.Core.BehaviorScript;
using Lunalipse.Presentation.LpsWindow;
using System.Windows.Threading;
using Lunalipse.Presentation.LpsComponent;
using System.Collections.Generic;
using Lunalipse.Common.Generic.AudioControlPanel;
using System.Windows.Media;
using Lunalipse.I18N;

namespace Lunalipse
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// （测试界面）
    /// </summary>
    public partial class MainWindow : LunalipseDialogue
    {
        MusicListPool mlp;
        MediaMetaDataReader mmdr;
        I18NConvertor converter;
        LpsAudio laudio;
        Interpreter intp;
        Dialogue dia;
        public MainWindow() : base()
        {
            InitializeComponent();
            mlp = MusicListPool.INSATNCE;
            laudio = LpsAudio.INSTANCE();
            converter = I18NConvertor.INSTANCE();
            mmdr = new MediaMetaDataReader(converter);
            mlp.AddToPool("F:/M2", mmdr);
            foreach (MusicEntity me in mlp.Musics)
            {
                dipMusic.Add(me);
            }
            intp = Interpreter.INSTANCE(@"F:\Lunalipse\TestUnit\bin\Debug");
            //if (intp.Load("prg2"))
            //{
            //    PlayFinished();
            //}
            //alb.Source = mlp.ToCatalogue().GetCatalogueCover();
            AudioDelegations.PlayingFinished += PlayFinished;
            AudioDelegations.MusicLoaded += MusicPerpeared;
            dipMusic.ItemSelectionChanged += DipMusic_ItemSelectionChanged;
            ControlPanel.OnTrigging += ControlPanel_OnTrigging;
            AudioDelegations.PostionChanged += NotifyChanged;
            ControlPanel.Value = 0;
            ControlPanel.OnProgressChanged += ControlPanel_OnProgressChanged;
            ControlPanel.OnVolumeChanged += ControlPanel_OnVolumeChanged;
        }

        private void ControlPanel_OnVolumeChanged(double value)
        {
            laudio.Volume = (float)value;
        }

        private void ControlPanel_OnProgressChanged(double value)
        {
            laudio.MoveTo(value);
        }

        private void ControlPanel_OnTrigging(AudioPanelTrigger identifier, object args)
        {
            switch (identifier)
            {
                case AudioPanelTrigger.PausePlay:
                    bool isPaused = (bool)args;
                    if (laudio.isLoaded)
                    {
                        if (isPaused) laudio.Pause();
                        else laudio.Resume();
                    }
                    break;
            }
        }

        private void NotifyChanged(TimeSpan curPos)
        {
            Dispatcher.Invoke(()=>
            {
                ControlPanel.Value = curPos.TotalSeconds;
                ControlPanel.Current = curPos;
            });
        }

        private void MusicPerpeared(MusicEntity Music, Track mTrack)
        {
            Dispatcher.Invoke(() =>
            {
                ControlPanel.MaxValue = mTrack.Duration.TotalSeconds;
                ControlPanel.Value = 0;
            });
        }

        private void DipMusic_ItemSelectionChanged(MusicEntity selected)
        {
            if (laudio.Playing) laudio.Stop();
            ControlPanel.AlbumProfile = new ImageBrush(MediaMetaDataReader.GetPicture(selected.Path));
            laudio.Load(selected);
            ControlPanel.StartPlaying();
            laudio.Play();
            //if (dia == null)
            //{
            //    dia = new Dialogue(new _3DVisualize(), "3D");
            //    dia.Show();
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.EnableBlur();
            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        if(laudio.Playing)
            //            Console.WriteLine(string.Join(",", AudioDelegations.FftAcquired()));
            //        Thread.Sleep(100);
            //    }
            //});
        }


        private void Window_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            laudio.Dispose();
        }

        private void PlayFinished()
        {
            MusicEntity MEnt = null;
            if (intp.LBSLoaded)
                MEnt = intp.Stepping();
            else
            {
                Next(true);
            }
        }

        private void Next(bool proccedNext)
        {
            Dispatcher.Invoke(() => dipMusic.SelectedIndex++);
        }

        private void EventTrigger_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}
