using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.IMetadata
{
    public interface IMediaMetadataWriter
    {
        bool SetArtist(int index, string artist);
        void SetTitle(string title);
        void SetAlbum(string album);
        bool SetPicture(int index, byte[] picture);
        void SetYear(uint year);
        void Done();
    }
}
