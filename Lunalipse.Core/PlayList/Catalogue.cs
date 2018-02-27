using Lunalipse.Common.Data;
using Lunalipse.Common.Interfaces.IPlayList;
using Lunalipse.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Lunalipse.Core.PlayList
{
    public class Catalogue : ICatalogue
    {
        private List<MusicEntity> Entities;
        private int Currently = -1;

        [XmlAttribute]
        public string Name { get; private set; }
        [XmlAttribute]
        public string UUID { get; private set; }
        public bool isAlbumClassified { get; set; }
        public bool MainCatalogue { get; private set; }
        public List<MusicEntity> MusicList
        {
            get
            {
                return Entities;
            }
        }

        public Catalogue()
        {

        }

        public Catalogue(bool isMainUses = false)
        {
            UUID = Guid.NewGuid().ToString();
            MainCatalogue = isMainUses;
        }
        public Catalogue(string Name, bool isMainUses = false)
            : this(isMainUses)
        {
            this.Name = Name;
            Entities = new List<MusicEntity>();
            if (!isMainUses) MusicListPool.OnMusicDeleted += DeleteMusic;
        }
        public Catalogue(List<MusicEntity> list, string Name, bool isMainUses = false)
            : this(isMainUses)
        {
            this.Name = Name;
            Entities = list;
            if (!isMainUses) MusicListPool.OnMusicDeleted += DeleteMusic;
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
                Entities.Sort((a, b) => a.Album.CompareTo(b.Album));
            }
        }

        public void SortByName()
        {
            Entities.Sort((a, b) => a.Name.CompareTo(b.Name));
        }

        public void SortByYear()
        {
            Entities.Sort((a, b) => a.Year.CompareTo(b.Year));
        }

        public bool DeleteMusic(string name)
        {
            return Entities.Remove(Entities.Find(e => e.Name == name));
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
            Currently = index;
            return Entities[index];
        }

        public List<MusicEntity> SearchMusic(string name)
        {
            return Entities.FindAll(e => e.Name.Contains(name));
        }

        public MusicEntity getMusic(string name)
        {
            MusicEntity me = Entities.Find(e => e.Name == name);
            Currently = Entities.IndexOf(me);
            return me;
        }

        public MusicEntity getNext()
        {
            Currently++;
            if (Currently < Entities.Count)
            {
                return Entities[Currently];
            }
            Currently = -1;
            return null;
        }

        public BitmapSource GetCatalogueCover()
        {
            Random r = new Random();
            int failTime = 0;
            MusicEntity randomed = Entities[r.Next(0, Entities.Count)];
            BitmapSource bs;
            while((bs = MediaMetaDataReader.GetPicture(randomed.Path))==null && failTime<3)
            {
                failTime++;
            }
            return bs;
        }
    }
}
