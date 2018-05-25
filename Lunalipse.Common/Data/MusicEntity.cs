using System.Runtime.Serialization;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Lunalipse.Common.Data
{
    public class MusicEntity 
    {
        public string Album, Name, ID3Name, Extension, Path, Year;
        public string[] Artist;

        public string ArtistFrist
        {
            get
            {
                return Artist.Length > 0 ? Artist[0] : "CORE_PRESENTOR_UNKNOW_ARTIST";
            }
        }

        // Use file name as music name
        public string MusicName
        {
            get
            {
                return Name;
            }
        }

        // Name stored in ID3v2 tag
        public string IDv23Name
        {
            get
            {
                return ID3Name;
            }
        }
    }
}
