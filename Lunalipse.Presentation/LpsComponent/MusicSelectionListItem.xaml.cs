using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Lunalipse.Common.Data;
using Lunalipse.Presentation.Generic;

namespace Lunalipse.Presentation.LpsComponent
{
    /// <summary>
    /// MusicSelectionListItem.xaml 的交互逻辑
    /// </summary>
    public partial class MusicSelectionListItem : UserControl
    {
        public static readonly DependencyProperty ITEM_SONGS =
            DependencyProperty.Register("MUSICLST_ITEM_SONGSTNAME",
                                        typeof(Brush),
                                        typeof(MusicSelectionList),
                                        new PropertyMetadata(Application.Current.FindResource("MusicName_List")));
        public static readonly DependencyProperty ITEM_ARTIST =
            DependencyProperty.Register("MUSICLST_ITEM_ARTISTNAME",
                                        typeof(Brush),
                                        typeof(MusicSelectionList),
                                        new PropertyMetadata(Application.Current.FindResource("ArtistName_List")));
        public static readonly DependencyProperty ITEM_SELECTED_MARK =
            DependencyProperty.Register("MUSICLST_ITEM_SELECTED_MARK",
                                        typeof(Brush),
                                        typeof(MusicSelectionList),
                                        new PropertyMetadata(Application.Current.FindResource("ArtistName_List")));

        public SolidColorBrush PrimaryColor
        {
            get => (SolidColorBrush)GetValue(ITEM_SONGS);
            set => SetValue(ITEM_SONGS, value);
        }
        public SolidColorBrush SecondaryColor
        {
            get => (SolidColorBrush)GetValue(ITEM_ARTIST);
            set => SetValue(ITEM_ARTIST, value);
        }
        /// <summary>
        /// 选中标识颜色
        /// </summary>
        public SolidColorBrush MarkColor
        {
            get => (SolidColorBrush)GetValue(ITEM_SELECTED_MARK);
            set => SetValue(ITEM_ARTIST, value);
        }

        public MusicSelectionListItem()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Delegation.RemovingItem?.Invoke((sender as Button).DataContext);
        }

        public void SetChosen()
        {
            Tag = true;
            SelectedMark.Background = MarkColor;
        }

        public void RemoveChosen()
        {
            Tag = false;
            SelectedMark.Background = null;
        }

    }
}
