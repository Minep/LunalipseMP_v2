using Lunalipse.Common.Interfaces.II18N;
using Lunalipse.Common.Interfaces.IPlayList;
using Lunalipse.Common.Generic.Catalogue;
using Lunalipse.Presentation.Generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// CatalogueSelectionList.xaml 的交互逻辑
    /// </summary>
    public partial class CatalogueSelectionList : UserControl, ITranslatable
    {
        private int __index = -5;
        private CatalogueSections TAG;
        private ObservableCollection<ICatalogue> Items = new ObservableCollection<ICatalogue>();

        /// <summary>
        /// 选项更改事件。参数tag的类型是<see cref="CatalogueSections"/>
        /// </summary>
        public event OnItemSelected<ICatalogue> OnSelectionChange;

        public CatalogueSelectionList()
        {
            InitializeComponent();
            ITEMS.DataContext = Items;
        }
        public static readonly DependencyProperty ITEM_HOVER =
            DependencyProperty.Register("CATALIST_HOVERCOLOR",
                                        typeof(Brush),
                                        typeof(CatalogueSelectionList),
                                        new PropertyMetadata(Application.Current.FindResource("ItemHoverColorDefault")));
        public static readonly DependencyProperty ITEM_UNHOVER =
            DependencyProperty.Register("CATALIST_UNHOVERCOLOR",
                                        typeof(Brush),
                                        typeof(CatalogueSelectionList),
                                        new PropertyMetadata(Application.Current.FindResource("ItemUnhoverColorDefault")));

        public SolidColorBrush ItemHovered
        {
            get => (SolidColorBrush)GetValue(ITEM_HOVER);
            set => SetValue(ITEM_HOVER, value);
        }
        public SolidColorBrush ItemUnhovered
        {
            get => (SolidColorBrush)GetValue(ITEM_UNHOVER);
            set => SetValue(ITEM_UNHOVER, value);
        }

        public void Reset()
        {
            Items.Clear();
            TipMessage.Visibility = Visibility.Hidden;
        }
        public void Add(ICatalogue catalogue) => Items.Add(catalogue);
        public void RemoveAt(int index) => Items.RemoveAt(index);

        /// <summary>
        /// 当没有东西能被添加到List里时调用此方法。
        /// </summary>
        public void EmptyContent()
        {
            Items.Clear();
            TipMessage.Visibility = Visibility.Visible;
        }

        public ICatalogue TheMainCatalogue
        {
            set => MainCatalogue.DataContext = value;
            get => MainCatalogue.DataContext as ICatalogue;
        }

        public int SelectedIndex
        {
            get => __index;
            set
            {
                CatalogueSelectionListItem csli = null;
                if (value == -1)
                {
                    csli = MainCatalogue;
                    TAG = CatalogueSections.ALL_MUSIC;
                }
                else if (value == -2)
                {
                    csli = AlbumCollection;
                    TAG = CatalogueSections.ALBUM_COLLECTIONS;
                }
                else if (value == -3)
                {
                    csli = UserPlaylist;
                    TAG = CatalogueSections.USER_PLAYLISTS;
                }
                else if (value == -4)
                {
                    csli = ArtistCollection;
                    TAG = CatalogueSections.ARTIST_COLLECTIONS;
                }
                else if (value >= 0)
                {
                    csli = GetContainer(value);
                }
                if (csli != null)
                {
                    ICatalogue ict = csli.DataContext as ICatalogue;
                    if (__index != -5)
                    {
                        // For Item in listbox
                        if (__index >= 0)
                        {
                            GetContainer(__index).SetUnselected();
                        }
                        else if (__index == -1)
                            MainCatalogue.SetUnselected();
                        else if (__index == -2)
                            AlbumCollection.SetUnselected();
                        else if (__index == -3)
                            UserPlaylist.SetUnselected();
                        else if (__index == -4)
                            ArtistCollection.SetUnselected();
                    }
                    csli.SetSelected();
                    __index = value;
                    OnSelectionChange?.Invoke(SelectedItem = ict, TAG);
                }
            }
        }
        public ICatalogue SelectedItem { get; private set; }

        private void ItemConatiner_MouseDown(object sender, MouseButtonEventArgs args)
        {
            CatalogueSelectionListItem csli = (CatalogueSelectionListItem)sender;
            ICatalogue ict = csli.DataContext as ICatalogue;
            if (csli != null)
            {
                if (__index != -5)
                {
                    // For Item in listbox
                    if (__index >=0)
                        GetContainer(__index).SetUnselected();
                    else if (__index == -1)
                        MainCatalogue.SetUnselected();
                    else if (__index == -2)
                        AlbumCollection.SetUnselected();
                    else if (__index == -3)
                        UserPlaylist.SetUnselected();
                    else if (__index == -4)
                        ArtistCollection.SetUnselected();
                }
                //MAINCATA FOR INDEX -1
                if (csli.Tag != null)
                {
                    switch ((string)csli.Tag)
                    {
                        case "MAINCATA":
                            __index = -1;
                            TAG = CatalogueSections.ALL_MUSIC;
                            break;
                        case "ALBUM_COLLECTION":
                            __index = -2;
                            TAG = CatalogueSections.ALBUM_COLLECTIONS;
                            break;
                        case "USER_PLAYLIST":
                            __index = -3;
                            TAG = CatalogueSections.USER_PLAYLISTS;
                            break;
                        case "ARTIST_COLLECTION":
                            __index = -4;
                            TAG = CatalogueSections.ARTIST_COLLECTIONS;
                            break;
                    }
                }
                else
                {
                    __index = Items.IndexOf(ict);
                    TAG = CatalogueSections.INDIVIDUAL;
                }
                csli.SetSelected();
                OnSelectionChange?.Invoke(SelectedItem = ict, TAG);
            }
        }

        private CatalogueSelectionListItem GetContainer(int index)
        {
            var container = (ITEMS.ItemContainerGenerator
                        .ContainerFromIndex(index) as FrameworkElement);
            return ITEMS.ItemTemplate.FindName("ItemConatiner", container) as CatalogueSelectionListItem;
        }

        public void Translate(II18NConvertor i8c)
        {
            MainCatalogue.CatalogueText = i8c.ConvertTo("CORE_FUNC", MainCatalogue.CatalogueText);
            AlbumCollection.CatalogueText = i8c.ConvertTo("CORE_FUNC", "CORE_CATALOGUE_ALBUM");
            UserPlaylist.CatalogueText = i8c.ConvertTo("CORE_FUNC", "CORE_CATALOGUE_PLAYLIST");
            TipMessage.Content = i8c.ConvertTo("CORE_FUNC", (string)TipMessage.Content);
            ArtistCollection.CatalogueText = i8c.ConvertTo("CORE_FUNC", "CORE_CATALOGUE_ARTSIT");
        }
    }
}
