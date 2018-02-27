using System.Collections.Generic;
using Lunalipse.Common.Data;
using System.IO;
using Lunalipse.Common.Interfaces.IPlayList;
using Lunalipse.Common.Interfaces.IMetadata;
using Lunalipse.Core.Console;
using Lunalipse.Common.Interfaces.IConsole;

namespace Lunalipse.Core.PlayList
{
    internal delegate bool MusicDeleted(string uuid);
    public class MusicListPool : ComponentHandler, IMusicListPool
    {
        static volatile MusicListPool mlpInstance;
        static readonly object mlpLock = new object();
        internal static event MusicDeleted OnMusicDeleted;

        public static MusicListPool INSATNCE
        {
            get
            {
                if(mlpInstance == null)
                {
                    lock(mlpLock)
                    {
                        mlpInstance = mlpInstance ?? new MusicListPool();
                    }
                }
                return mlpInstance;
            }
        }

        private CataloguePool CPool;
        private Catalogue AllMusic;
        public List<MusicEntity> Musics
        {
            get
            {
                return AllMusic.MusicList;
            }
        }

        private MusicListPool()
        {
            CPool = CataloguePool.INSATNCE;
            AllMusic = new Catalogue("CORE_CATALOGUE_AllMusic", true);
            CPool.AddCatalogue(AllMusic);
            ConsoleAdapter.INSTANCE.RegisterComponent("lpslist", this);
        }

        public void AddToPool(string dirpath, IMediaMetadataReader immr)
        {
            foreach(string fi in Directory.GetFiles(dirpath))
            {
                if(SupportFormat.AllQualified(Path.GetExtension(fi)))
                {
                    AllMusic.AddMusic(immr.CreateEntity(fi));
                }
            }
        }

        public void AddToPool(string[] pathes, IMediaMetadataReader immr)
        {
            foreach(string s in pathes)
            {
                foreach (string fi in Directory.GetFiles(s))
                {
                    if (SupportFormat.AllQualified(Path.GetExtension(fi)))
                    {
                        AllMusic.AddMusic(immr.CreateEntity(fi));
                    }
                }
            }
        }

        public void DeleteMusic(MusicEntity entity, bool complete)
        {
            if (complete) File.Delete(entity.Path);
            OnMusicDeleted?.Invoke(entity.Name);
            AllMusic.DeleteMusic(entity);
        }

        public bool AddFileToPool(string MediaPath, IMediaMetadataReader immr)
        {
            if (SupportFormat.AllQualified(Path.GetExtension(MediaPath)))
            {
                AllMusic.AddMusic(immr.CreateEntity(MediaPath));
                return true;
            }
            return false;
        }

        public List<MusicEntity> GetMusics(string any, MusicEntityType mety)
        {
            return AllMusic.MusicList.FindAll(delegate (MusicEntity e)
            {
                switch (mety)
                {
                    case MusicEntityType.ARTIST:
                        return e.Artist[0].Equals(any);
                    case MusicEntityType.ALBUM:
                        return e.Album.Equals(any);
                    case MusicEntityType.NAME:
                        return e.Name.Equals(any);
                    default:
                        return false;
                }
            });
        }

        public MusicEntity GetMusic(int index)
        {
            return index > AllMusic.MusicList.Count - 1 ? null : AllMusic.MusicList[index];
        }

        public ICatalogue ToCatalogue()
        {
            return AllMusic;
        }
    }
}
