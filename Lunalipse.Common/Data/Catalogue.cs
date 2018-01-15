using Lunalipse.Common.Data;
using Lunalipse.Common.Interfaces.IPlayList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Data
{
    public class Catalogue : ICatalogue
    {
        private List<MusicEntity> Entities;

        public string Name { get; private set; }
        public bool isAlbumClassified { get; set; }

        public Catalogue(string Name)
        {
            this.Name = Name;
            Entities = new List<MusicEntity>();
        }
        public Catalogue(List<MusicEntity> list, string Name)
        {
            this.Name = Name;
            Entities = list;
        }

        public bool AddMusic(MusicEntity ME)
        {
            foreach(MusicEntity me in Entities)
            {
                if (me.Name.Equals(ME.Name)) return false;
            }
            Entities.Add(ME);
            return true;
        }

        public void SortByAlbum()
        {
            if(!isAlbumClassified)
            {
                Entities.Sort(delegate (MusicEntity a, MusicEntity b)
                {
                    return a.Album.CompareTo(b.Album);
                });
            }
        }

        public void SortByName()
        {
            Entities.Sort(delegate (MusicEntity a, MusicEntity b)
            {
                return a.Name.CompareTo(b.Name);
            });
        }

        public void SortByYear()
        {
            Entities.Sort(delegate (MusicEntity a, MusicEntity b)
            {
                return a.Year.CompareTo(b.Year);
            });
        }

        public bool DeleteMusic(string name)
        {
            return Entities.Remove(Entities.Find(delegate (MusicEntity e)
            {
                return e.Name == name;
            }));
        }

        public bool DeleteMusic(int index)
        {
            if (index > Entities.Count - 1) return false;
            Entities.RemoveAt(index);
            return true;
        }

        public bool DeleteMusic(MusicEntity ME)
        {
            return Entities.Remove(ME);
        }

        public bool DeleteMusic(int start, int count)
        {
            if (start < 0 || start + count > Entities.Count) return false;
            Entities.RemoveRange(start, count);
            return true;
        }

        public int GetCount()
        {
            return Entities.Count;
        }

        public MusicEntity getMusic(int index)
        {
            if (index > Entities.Count - 1) return null;
            return Entities[index];
        }

        public List<MusicEntity> SearchMusic(string name)
        {
            return Entities.FindAll(delegate (MusicEntity e)
            {
                return e.Name == name;
            });
        }

        public bool DeleteMusicByUUID(string uuid)
        {
            return Entities.Remove(Entities.Find(delegate (MusicEntity e)
            {
                return e.id == uuid;
            }));
        }

        public MusicEntity getMusic(string uuid)
        {
            return Entities.Find(delegate (MusicEntity e)
            {
                return e.id == uuid;
            });
        }
    }
}
