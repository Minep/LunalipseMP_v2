using System.Collections.Generic;
using Lunalipse.Common.Data;
using System.IO;
using Lunalipse.Common.Interfaces.IPlayList;
using Lunalipse.Common.Interfaces.IMetadata;

namespace Lunalipse.Core.PlayList
{
    public class MusicListPool : IMusicListPool
    {
        static volatile MusicListPool mlpInstance;
        static readonly object mlpLock = new object();

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

        private List<MusicEntity> entities_pool;
        public List<MusicEntity> Musics
        {
            get
            {
                return entities_pool;
            }
        }
        public MusicListPool()
        {
            entities_pool = new List<MusicEntity>();
        }
        public void AddToPool(string dirpath, IMediaMetadataReader immr)
        {
            foreach(string fi in Directory.GetFiles(dirpath))
            {
                if(SupportFormat.AllQualified(Path.GetExtension(fi)))
                {
                    entities_pool.Add(immr.CreateEntity(fi));
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
                        entities_pool.Add(immr.CreateEntity(fi));
                    }
                }
            }
        }

        public void DeleteMusic(MusicEntity entity, bool complete)
        {
            if (complete) File.Delete(entity.Path);
            entities_pool.Remove(entity);
        }

        public bool AddFileToPool(string MediaPath, IMediaMetadataReader immr)
        {
            if (SupportFormat.AllQualified(Path.GetExtension(MediaPath)))
            {
                entities_pool.Add(immr.CreateEntity(MediaPath));
                return true;
            }
            return false;
        }

        public List<MusicEntity> GetMusics(string any, MusicEntityType mety)
        {
            return entities_pool.FindAll(delegate (MusicEntity e)
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
            return index > entities_pool.Count - 1 ? null : entities_pool[index];
        }

        public MusicEntity GetMusicByUUID(string uuid)
        {
            return entities_pool.Find(delegate (MusicEntity e)
            {
                return e.id.Equals(uuid);
            });
        }
    }
}
