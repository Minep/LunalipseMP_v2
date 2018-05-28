using System;
using System.ComponentModel;
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
            SelectedMark.Background = FindResource("SongChosen_List") as SolidColorBrush;
        }

        public void RemoveChosen()
        {
            Tag = false;
            SelectedMark.Background = null;
        }

    }
}
