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
using Lunalipse.Core.I18N;
using Lunalipse.Utilities;
using System.Windows.Media.Imaging;
using Lunalipse.Common.Interfaces.IPlayList;
using Lunalipse.Common.Generic.Catalogue;
using Lunalipse.Presentation.Utils;
using Lunalipse.Core.Cache;
using Lunalipse.Common.Generic.Cache;
using static Lunalipse.Common.Generic.Cache.CacheInfo;

namespace Lunalipse
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// （测试界面）
    /// </summary>
    public partial class MainWindow : LunalipseMainWindow
    {
        MusicListPool mlp;
        CataloguePool CPOOL;
        MediaMetaDataReader mmdr;
        I18NConvertor converter;
        LpsAudio laudio;
        Interpreter intp;
        Dialogue dia;
        CacheHub cacheSystem;
        public MainWindow() : base()
        {
            InitializeComponent();
            InitializeModules();
        }

        private void DoTranslate()
        {
            CATALOGUES.Translate(converter);
            dipMusic.Translate(converter);
        }

        private void InitializeModules()
        {
            mlp = MusicListPool.INSATNCE;
            CPOOL = CataloguePool.INSATNCE;
            laudio = LpsAudio.INSTANCE();
            cacheSystem = CacheHub.INSTANCE(Environment.CurrentDirectory);
            converter = I18NConvertor.INSTANCE(I18NPages.INSTANCE);
            //mmdr = new MediaMetaDataReader(converter);
            //mlp.AddToPool("F:/M2", mmdr);

            //intp = Interpreter.INSTANCE(@"F:\Lunalipse\TestUnit\bin\Debug");
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
            laudio.Volume = (float)ControlPanel.Value;
            ControlPanel.OnProgressChanged += ControlPanel_OnProgressChanged;
            ControlPanel.OnVolumeChanged += ControlPanel_OnVolumeChanged;
            CATALOGUES.OnSelectionChange += CATALOGUES_OnSelectionChange;
            CATALOGUES.TheMainCatalogue = mlp.ToCatalogue();
            cacheSystem.RegisterOperator(CacheType.MUSIC_CATALOGUE_CACHE, new MusicCacheIndexer()
            {
                UseLZ78Compress = true
            });
        }

        private void CATALOGUES_OnSelectionChange(ICatalogue selected, object tag)
        {
            Catalogue cat = selected as Catalogue;
            CatalogueSections TAG = (CatalogueSections)tag;
            switch (TAG)
            {
                case CatalogueSections.ALL_MUSIC:
                    dipMusic.Clear();
                    dipMusic.UseCache(true);
                    break;
                case CatalogueSections.INDIVIDUAL:
                    dipMusic.Clear();
                    dipMusic.WaitOnUI(() =>
                    {
                        foreach (MusicEntity me in cat.MusicList)
                        {
                            Dispatcher.Invoke(() => dipMusic.Add(me));
                        }
                    });
                    dipMusic.UseCache(false);
                    break;
                case CatalogueSections.USER_PLAYLISTS:
                    CATALOGUES.EmptyContent();
                    break;
                case CatalogueSections.ALBUM_COLLECTIONS:
                    CATALOGUES.Reset();
                    List<Catalogue> catas = CPOOL.GetAlbumClassfied();
                    if (catas.Count == 0)
                    {
                        CATALOGUES.EmptyContent();
                    }
                    else
                    {
                        foreach (Catalogue c in catas)
                            CATALOGUES.Add(c);
                    }
                    break;
                case CatalogueSections.ARTIST_COLLECTIONS:
                    CATALOGUES.Reset();
                    List<Catalogue> art_catas = CPOOL.GetArtistClassfied();
                    if (art_catas.Count == 0)
                    {
                        CATALOGUES.EmptyContent();
                    }
                    else
                    {
                        foreach (Catalogue c in art_catas)
                            CATALOGUES.Add(c);
                    }
                    break;
            }
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

        private void DipMusic_ItemSelectionChanged(MusicEntity selected, object tag)
        {
            if (laudio.Playing) laudio.Stop();
            BitmapSource source;
            ControlPanel.AlbumProfile = (source = MediaMetaDataReader.GetPicture(selected.Path)) == null ? null : new ImageBrush(source);
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
            foreach (Catalogue c in cacheSystem.RestoreObjects<Catalogue>(
                x => x.markName == "CATALOGUE",
                CacheType.MUSIC_CATALOGUE_CACHE
                ))
            {
                CPOOL.AddCatalogue(c);
            }
            DoTranslate();
            CATALOGUES.SelectedIndex = -1;
            //cacheSystem.CacheObject(CPOOL, CacheType.MUSIC_CATALOGUE_CACHE);
            dipMusic.WaitOnUI(() =>
            {
                mlp.CreateAlbumClasses();
                mlp.CreateArtistClasses();
                foreach (MusicEntity me in mlp.Musics)
                {
                    this.Dispatcher.Invoke(() => dipMusic.AddToCache(me));
                }
            });
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
