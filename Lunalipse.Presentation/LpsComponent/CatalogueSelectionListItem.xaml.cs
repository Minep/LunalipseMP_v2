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
    /// CatalogueSelectionListItem.xaml 的交互逻辑
    /// </summary>
    public partial class CatalogueSelectionListItem : UserControl
    {
        public SolidColorBrush SelectedColor { get; set; }
        public SolidColorBrush DefaultColor { get; set; }
        private bool isSelected=false;
        public bool Selected => isSelected;
        public CatalogueSelectionListItem()
        {
            InitializeComponent();
            SelectedColor = new BrushConverter().ConvertFromString("#ff233c7c") as SolidColorBrush;
            DefaultColor = Brushes.Transparent;
        }

        public string CatalogueText
        {
            get => Text.Text;
            set => Text.Text = value;
        }

        public void SetSelected()
        {
            TagIcon.Background = SelectedColor;
            isSelected = true;
        }
        public void SetUnselected()
        {
            TagIcon.Background = DefaultColor;
            isSelected = false;
        }

        private void CATALOGUE_LIST_ITEM_Loaded(object sender, RoutedEventArgs e)
        {
            if (Tag != null)
            {
                switch ((string)Tag)
                {
                    case "ALBUM_COLLECTION":
                        TagIcon.Content = FindResource("Album");
                        break;
                    case "USER_PLAYLIST":
                        TagIcon.Content = FindResource("Favorite");
                        break;
                    case "ARTIST_COLLECTION":
                        TagIcon.Content = FindResource("Artist");
                        break;
                }
            }
            else
                TagIcon.Content = FindResource("Favorite_Outline");
        }
    }
}
