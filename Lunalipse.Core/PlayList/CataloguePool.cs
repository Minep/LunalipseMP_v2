using Lunalipse.Common.Data;
using Lunalipse.Common.Interfaces.IConsole;
using Lunalipse.Common.Interfaces.IPlayList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Core.PlayList
{
    public class CataloguePool : ComponentHandler, ICataloguePool<Catalogue>
    {
        static volatile CataloguePool cpinstance;
        static readonly object cpLock = new object();
        public static CataloguePool INSATNCE
        {
            get
            {
                if (cpinstance == null)
                {
                    lock (cpLock)
                    {
                        cpinstance = cpinstance ?? new CataloguePool();
                    }
                }
                return cpinstance;
            }
        }

        List<Catalogue> CatalogueBase = new List<Catalogue>();

        public void AddCatalogue(Catalogue catalogue)
        {
            CatalogueBase.Add(catalogue);
        }

        public bool RemoveCatalogue(Catalogue catalogue)
        {
            return CatalogueBase.Remove(catalogue);
        }

        public void RemoveCatalogueRange(string keyword)
        {
            CatalogueBase.RemoveAll(x => x.Name.Contains(keyword));
        }

        public void RemoveCatalogue(string Uuid)
        {
            CatalogueBase.RemoveAll(x => x.UUID.Equals(Uuid));
        }

        public List<Catalogue> SearchCatalogue(string Name)
        {
            return CatalogueBase.FindAll(x => x.Name.Equals(Name));
        }

        public Catalogue GetCatalogue(string uuid)
        {
            return CatalogueBase.Find(x => x.UUID.Equals(uuid));
        }

        public void AddMusic(string uuid, MusicEntity music)
        {
            CatalogueBase.Find(x => x.UUID.Equals(uuid)).AddMusic(music);
        }

        public void RemoveMusic(string uuid, MusicEntity music)
        {
            CatalogueBase.Find(x => x.UUID.Equals(uuid)).DeleteMusic(music);
        }

        public void RemoveMusic(string uuid, string MusicUUID)
        {
            CatalogueBase.Find(x => x.UUID.Equals(uuid)).DeleteMusicByUUID(MusicUUID);
        }

        public void RemoveMusicRange(string uuid, string Name)
        {
            CatalogueBase.Find(x => x.UUID.Equals(uuid)).DeleteMusic(Name);
        }
    }
}
