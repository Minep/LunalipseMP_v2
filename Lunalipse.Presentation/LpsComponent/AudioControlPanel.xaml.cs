using Lunalipse.Common.Generic.AudioControlPanel;
using Lunalipse.Presentation.Generic;
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

namespace Lunalipse.Presentation.LpsComponent
{
    /// <summary>
    /// AudioControlPanel.xaml 的交互逻辑
    /// </summary>
    public partial class AudioControlPanel : UserControl
    {
        bool isPaused = false;
        bool isFullscreen = false;
        bool LyricEnabled = true;
        bool SpectrumEnabled = true;
        PlayMode Mode = PlayMode.RepeatList;

        /// <summary>
        /// 开关类事件触发，对于<see cref="AudioPanelTrigger.PausePlay"/>事件，<see cref="object"/>为<see cref="bool"/>，表示是否已暂停。
        /// </summary>
        public event AudioPanelDelegation<AudioPanelTrigger, object> OnTrigging;
        public event AudioPanelDelegation<PlayMode, object> OnModeChange;

        public AudioControlPanel()
        {
            InitializeComponent();
        }

        public Brush AlbumProfile
        {
            get => AlbProfile.Background;
            set => AlbProfile.Background = value;
        }

        private void SkipToPrevious(object sender, RoutedEventArgs e)
        {
            OnTrigging?.Invoke(AudioPanelTrigger.SkipPrev, null);
        }

        private void PlayOrPause(object sender, RoutedEventArgs e)
        {
            Button bsender = sender as Button;
            if (!isPaused)
            {
                bsender.Content = FindResource("Pause");
                isPaused = true;
            }
            else
            {
                bsender.Content = FindResource("Play");
                isPaused = false;
            }
            OnTrigging?.Invoke(AudioPanelTrigger.PausePlay, isPaused);
        }

        private void SkipToNext(object sender, RoutedEventArgs e)
        {
            OnTrigging?.Invoke(AudioPanelTrigger.SkipNext, null);
        }

        private void ChangePlayMode(object sender, RoutedEventArgs e)
        {
            Button bsender = sender as Button;
            switch (Mode)
            {
                case PlayMode.RepeatList:
                    Mode = PlayMode.RepeatOne;
                    bsender.Content = FindResource("RepeatOne");
                    break;
                case PlayMode.RepeatOne:
                    Mode = PlayMode.Shuffle;
                    bsender.Content = FindResource("Shuffle");
                    break;
                case PlayMode.Shuffle:
                    Mode = PlayMode.RepeatList;
                    bsender.Content = FindResource("RepeatList");
                    break;
            }
            OnModeChange(Mode, null);
        }

        private void LBScriptEnable(object sender, RoutedEventArgs e)
        {
            OnTrigging?.Invoke(AudioPanelTrigger.LBScript, null);
        }
        private void OpenEqualizer(object sender, RoutedEventArgs e)
        {
            OnTrigging?.Invoke(AudioPanelTrigger.Equalizer, null);
        }

        private void TriggerSpectrum(object sender, RoutedEventArgs e)
        {
            if (SpectrumEnabled)
            {
                SpectrumDisable.Visibility = Visibility.Visible;
                SpectrumEnabled = false;
            }
            else
            {
                SpectrumDisable.Visibility = Visibility.Hidden;
                SpectrumEnabled = true;
            }
            OnTrigging?.Invoke(AudioPanelTrigger.Spectrum, null);
        }
        private void TriggerLyric(object sender, RoutedEventArgs e)
        {
            if (LyricEnabled)
            {
                LyricDisable.Visibility = Visibility.Visible;
                LyricEnabled = false;
            }
            else
            {
                LyricDisable.Visibility = Visibility.Hidden;
                LyricEnabled = true;
            }
            OnTrigging?.Invoke(AudioPanelTrigger.Lyric, null);
        }
        private void TriggerFullScreen(object sender, RoutedEventArgs e)
        {
            Button bsender = sender as Button;
            if (isFullscreen)
            {
                bsender.Content = FindResource("FullScreen");
                isFullscreen = false;
            }
            else
            {
                bsender.Content = FindResource("ExitFullScreen");
                isFullscreen = true;
            }
            OnTrigging?.Invoke(AudioPanelTrigger.FullScreen, null);
        }
    }
}
