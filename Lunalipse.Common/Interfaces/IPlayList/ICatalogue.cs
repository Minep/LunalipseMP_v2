using Lunalipse.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.IPlayList
{
    public interface ICatalogue
    {
        bool AddMusic(MusicEntity ME);
        bool DeleteMusic(MusicEntity ME);
        bool DeleteMusic(string name);
        bool DeleteMusicByUUID(string uuid);
        bool DeleteMusic(int index);
        bool DeleteMusic(int start, int count);
        int GetCount();
        MusicEntity getMusic(int index);
        MusicEntity getMusic(string uuid);
        List<MusicEntity> SearchMusic(string name);
        void SortByYear();
        void SortByAlbum();
        void SortByName();
    }
}
