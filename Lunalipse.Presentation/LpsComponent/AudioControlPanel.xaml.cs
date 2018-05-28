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
using System.Windows.Media.Animation;
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
        public event ProgressChange OnProgressChanged;
        public event ProgressChange OnVolumeChanged;

        public AudioControlPanel()
        {
            InitializeComponent();
            MusicProgress.OnProgressChanged += x => OnProgressChanged?.Invoke(x);
            VolumeBar.OnValueChanged += x =>
            {
                if (x >= 25 && x < 75)
                {
                    VolumeAdj.Content = FindResource("Volume_025");
                }
                else if (x >= 75)
                {
                    VolumeAdj.Content = FindResource("Volume_075");
                }
                else if (x >= 1)
                {
                    VolumeAdj.Content = FindResource("Volume_0");
                }
                else
                {
                    VolumeAdj.Content = FindResource("Volume_off");
                }
                OnVolumeChanged?.Invoke(x);
            };
        }

        public Brush AlbumProfile
        {
            get => AlbProfile.Background;
            set => AlbProfile.Background = value;
        }

        public double MaxValue
        {
            get => MusicProgress.MaxValue;
            set => MusicProgress.MaxValue = value;
        }
        public double Value
        {
            get => MusicProgress.Value;
            set => MusicProgress.Value = value;
        }

        public TimeSpan Current
        {
            set
            {
                Time.Content = value.ToString(@"hh\:mm\:ss");
            }
        }

        private void SkipToPrevious(object sender, RoutedEventArgs e)
        {
            OnTrigging?.Invoke(AudioPanelTrigger.SkipPrev, null);
        }

        public void StartPlaying()
        {
            Play.Content = FindResource("Pause");
            isPaused = false;
        }

        private void PlayOrPause(object sender, RoutedEventArgs e)
        {
            Button bsender = sender as Button;
            if (isPaused)
            {
                bsender.Content = FindResource("Pause");
                isPaused = false;
            }
            else
            {
                bsender.Content = FindResource("Play");
                isPaused = true;
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
            OnModeChange?.Invoke(Mode, null);
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            VolumeBar.MaxValue = 100;
            VolumeBar.Value = 10;
        }

        private void VolumePlanePopup_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!VolumeBar.IsHold)
            {
                VolumeBar.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.3))));
                VolumePlanePopup.IsOpen = false;
            }
        }
    }
}
