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
    /// LpsToggleBox.xaml 的交互逻辑
    /// </summary>
    public partial class LpsToggleBox : UserControl
    {
        private bool isToggleOn = false;

        public event RoutedEventHandler OnSwitchToggled;

        public static readonly DependencyProperty TB_BG_OFF =
            DependencyProperty.Register("TOGBX_THUMB_OFF",
                                        typeof(Brush),
                                        typeof(LpsToggleBox),
                                        new PropertyMetadata(Application.Current.FindResource("ToggleThumbOffDefault")));
        public static readonly DependencyProperty TRACK_BG_OFF =
            DependencyProperty.Register("TOGBX_TRACK_BACKGROUND_OFF",
                                        typeof(Brush),
                                        typeof(LpsToggleBox),
                                        new PropertyMetadata(Application.Current.FindResource("ToggleTrackOffDefault")));
        public static readonly DependencyProperty TB_BG_ON =
            DependencyProperty.Register("TOGBX_THUMB_ON",
                                        typeof(Brush),
                                        typeof(LpsToggleBox),
                                        new PropertyMetadata(Application.Current.FindResource("ToggleThumbOnDefault")));
        public static readonly DependencyProperty TRACK_BG_ON =
            DependencyProperty.Register("TOGBX_TRACK_BACKGROUND_ON",
                                        typeof(Brush),
                                        typeof(LpsToggleBox),
                                        new PropertyMetadata(Application.Current.FindResource("ToggleTrackOnDefault")));

        public Brush ThumbOff
        {
            get => GetValue(TB_BG_OFF) as Brush;
            set => SetValue(TB_BG_OFF, value);
        }
        public Brush ThumbOn
        {
            get => GetValue(TB_BG_ON) as Brush;
            set => SetValue(TB_BG_ON, value);
        }
        public Brush TrackOff
        {
            get => GetValue(TRACK_BG_OFF) as Brush;
            set => SetValue(TRACK_BG_OFF, value);
        }
        public Brush TrackOn
        {
            get => GetValue(TRACK_BG_ON) as Brush;
            set => SetValue(TRACK_BG_ON, value);
        }
        public bool IsToggleOn
        {
            get => isToggleOn;
        }
        public DoubleAnimation ToOnState = new DoubleAnimation(7, 21, new Duration(TimeSpan.FromMilliseconds(90)));
        public DoubleAnimation ToOffState = new DoubleAnimation(21, 7, new Duration(TimeSpan.FromMilliseconds(90)));
        public ColorAnimation ToOnStateColor, ToOffStateColor,ToOnStateColorThumb, ToOffStateColorThumb;

        public LpsToggleBox()
        {
            InitializeComponent();
            // Make a new wrap in case of 'Frozen Exception'
            Thumb.Fill = new SolidColorBrush((ThumbOff as SolidColorBrush).Color);
            OnTrack.Background= new SolidColorBrush((TrackOff as SolidColorBrush).Color);

            ToOnStateColor = new ColorAnimation((TrackOn as SolidColorBrush).Color, new Duration(TimeSpan.FromMilliseconds(90)));
            ToOffStateColor = new ColorAnimation((TrackOff as SolidColorBrush).Color, new Duration(TimeSpan.FromMilliseconds(90)));
            ToOnStateColorThumb = new ColorAnimation((ThumbOn as SolidColorBrush).Color, new Duration(TimeSpan.FromMilliseconds(90)));
            ToOffStateColorThumb = new ColorAnimation((ThumbOff as SolidColorBrush).Color, new Duration(TimeSpan.FromMilliseconds(90)));
        }

        private void OnToggled(object sender, RoutedEventArgs args)
        {
            if (isToggleOn)
            {
                OnTrack.BeginAnimation(WidthProperty, ToOffState);
                OnTrack.Background.BeginAnimation(SolidColorBrush.ColorProperty, ToOffStateColor);
                Thumb.Fill.BeginAnimation(SolidColorBrush.ColorProperty, ToOffStateColorThumb);
                isToggleOn = false;
            }
            else
            {
                OnTrack.BeginAnimation(WidthProperty, ToOnState);
                OnTrack.Background.BeginAnimation(SolidColorBrush.ColorProperty, ToOnStateColor);
                Thumb.Fill.BeginAnimation(SolidColorBrush.ColorProperty, ToOnStateColorThumb);
                isToggleOn = true;
            }
            OnSwitchToggled?.Invoke(sender, args);
        }
    }
}
