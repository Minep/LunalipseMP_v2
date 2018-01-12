using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunalipse.Common.Data;
using Lunalipse.Common.Interfaces.IPlayList;

namespace Lunalipse.Core.PlayList
{
    class CatalogueFactory : ICatalogueFactory
    {
        public Catalogue Create(string Name, bool isAlbumClassified)
        {
            return new Catalogue(Name)
            {
                isAlbumClassified = isAlbumClassified
            };
        }

        public Catalogue Create(string Name, bool isAlbumClassified, List<MusicEntity> entities)
        {
            return new Catalogue(entities, Name)
            {
                isAlbumClassified = isAlbumClassified
            };
        }
    }
}
