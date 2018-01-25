using Lunalipse.Common.Data;
using Lunalipse.Core.PlayList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.IPlayList
{
    public interface ICatalogueFactory
    {
        ICatalogue Create(string Name, bool isAlbumClassified);
        ICatalogue Create(string Name, bool isAlbumClassified, List<MusicEntity> entities);
    }
}
