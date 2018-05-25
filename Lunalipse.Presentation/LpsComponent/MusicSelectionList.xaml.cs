using Lunalipse.Common.Data;
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
    public delegate void OnItemSelected(MusicEntity selected);
    /// <summary>
    /// MusicSelectionList.xaml 的交互逻辑
    /// </summary>
    public partial class MusicSelectionList : UserControl
    {
        private ObservableCollection<MusicEntity> Items = new ObservableCollection<MusicEntity>();
        private int __index = -1;
        public event OnItemSelected ItemSelectionChanged;
        public MusicSelectionList()
        {
            InitializeComponent();
            ITEMS.DataContext = Items;
            Delegates.RemovingItem += dctx =>
            {
                MusicEntity removed = dctx as MusicEntity;
                if (dctx is MusicEntity)
                {
                    if (!IsMotherCatalogue)
                    {
                        Delegates.CatalogueUpdated(removed);
                        Items.Remove(removed);
                    }
                    else
                    {
                        // TODO 永久从母分类中删除歌曲（本地文件永久删除），包括：提醒
                    }
                }
            };
        }

        public void Add(MusicEntity mie)
        {
            Items.Add(mie);
        }
        public List<MusicEntity> CurrentItems => Items.ToList();

        public bool IsMotherCatalogue
        {
            get;
            set;
        }

        public MusicEntity SelectedItem { get; private set; }

        public int SelectedIndex
        {
            get => __index;
            set
            {
                if (__index == -1)
                {
                    for(int i=0;i<ITEMS.Items.Count;i++)
                    {
                        MusicSelectionListItem Container = GetContainer(i);
                        if ((Boolean)Container.Tag != false)
                        {
                            Container.RemoveChosen();
                            break;
                        }
                    }
                }
                else
                {
                    MusicSelectionListItem Temp = GetContainer(__index);
                    Temp.RemoveChosen();
                    Temp = GetContainer(__index = value);
                    Temp.SetChosen();
                    ItemSelectionChanged(Temp.DataContext as MusicEntity);
                }
            }
        }

        private void ItemConatiner_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MusicSelectionListItem Item = (MusicSelectionListItem)sender;
            MusicSelectionListItem Temp;
            MusicEntity selected = Item.DataContext as MusicEntity;
            if (__index != -1)
            {
                Temp = GetContainer(__index);
                Temp.RemoveChosen();

            }
            if (selected != null)
            {
                __index = Items.IndexOf(selected);
                Item.SetChosen();
                ItemSelectionChanged(SelectedItem = selected);
            }
        }

        private MusicSelectionListItem GetContainer(int index)
        {
            var container = (ITEMS.ItemContainerGenerator
                        .ContainerFromIndex(index) as FrameworkElement);
            return ITEMS.ItemTemplate.FindName("ItemConatiner",container) as MusicSelectionListItem;
        }
    }
}
