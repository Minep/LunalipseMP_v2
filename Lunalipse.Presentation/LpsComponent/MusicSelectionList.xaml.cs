﻿using Lunalipse.Common.Data;
using Lunalipse.Common.Generic;
using Lunalipse.Common.Interfaces.II18N;
using Lunalipse.Presentation.Generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Lunalipse.Presentation.LpsComponent
{
    /// <summary>
    /// MusicSelectionList.xaml 的交互逻辑
    /// </summary>
    public partial class MusicSelectionList : UserControl, ITranslatable, IWaitable
    {
        private ObservableCollection<MusicEntity> Items = new ObservableCollection<MusicEntity>();
        private ObservableCollection<MusicEntity> Cache = new ObservableCollection<MusicEntity>();
        private int __index = -1;
        public event OnItemSelected<MusicEntity> ItemSelectionChanged;

        public static readonly DependencyProperty ITEM_HOVER =
            DependencyProperty.Register("MUSICLST_HOVERCOLOR",
                                        typeof(Brush),
                                        typeof(MusicSelectionList),
                                        new PropertyMetadata(Application.Current.FindResource("ItemHoverColorDefault")));
        public static readonly DependencyProperty ITEM_UNHOVER =
            DependencyProperty.Register("MUSICLST_UNHOVERCOLOR",
                                        typeof(Brush),
                                        typeof(MusicSelectionList),
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

        public MusicSelectionList()
        {
            InitializeComponent();
            ITEMS.DataContext = Items;
            Delegation.RemovingItem += dctx =>
            {
                MusicEntity removed = dctx as MusicEntity;
                if (dctx is MusicEntity)
                {
                    if (!IsMotherCatalogue)
                    {
                        Delegation.CatalogueUpdated(removed);
                        Items.Remove(removed);
                    }
                    else
                    {
                        // TODO 永久从母分类中删除歌曲（本地文件永久删除），包括：提醒
                    }
                }
            };
        }

        public void Add(MusicEntity mie) => Items.Add(mie);
        public void AddToCache(MusicEntity mie) => Cache.Add(mie);
        public void Clear()
        {
            TipMessage.Visibility = Visibility.Hidden;
            Items.Clear();
        }
        public List<MusicEntity> CurrentItems => Items.ToList();
        public void UseCache(bool isuse)
        {
            if (isuse) ITEMS.DataContext = Cache;
            else ITEMS.DataContext = Items;
        }

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

        public void Translate(II18NConvertor i8c)
        {
            TipMessage.Content = i8c.ConvertTo("CORE_FUNC", (string)TipMessage.Content);
            WaitingHint.Content = i8c.ConvertTo("CORE_FUNC", (string)WaitingHint.Content);
        }

        public void StartWait()
        {
            Dispatcher.Invoke(()=> Loading.Visibility = Visibility.Visible);
        }

        public void StopWait()
        {
            Dispatcher.Invoke(() => Loading.Visibility = Visibility.Hidden);
        }
    }
}
