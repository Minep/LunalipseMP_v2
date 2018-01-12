using Lunalipse.Common.Interfaces.IMetadata;
using System;
using TL = TagLib;

namespace Lunalipse.Core.Metadata
{
    class MediaMetadataWriter : IMediaMetadataWriter
    {
        TL.File mfile;
        public MediaMetadataWriter(string path)
        {
            mfile = TL.File.Create(path);
        }
        public MediaMetadataWriter(TL.File fileInstance)
        {
            mfile = fileInstance;
        }

        public void Done()
        {
            mfile.Save();
            mfile.Dispose();
        }

        public void SetAlbum(string album)
        {
            mfile.Tag.Album = album;
        }

        public bool SetArtist(int index, string artist)
        {
            if (index > mfile.Tag.Performers.Length - 1) return false;
            mfile.Tag.Performers[index] = artist;
            return true;
        }

        public bool SetPicture(int index, byte[] picture)
        {
            if (index > mfile.Tag.Pictures.Length - 1) return false;
            mfile.Tag.Pictures[index] = new TL.Picture(new TL.ByteVector(picture));
            return true;
        }

        public void SetTitle(string title)
        {
            mfile.Tag.Title = title;
        }

        public void SetYear(uint year)
        {
            mfile.Tag.Year = year;
        }
    }
}
