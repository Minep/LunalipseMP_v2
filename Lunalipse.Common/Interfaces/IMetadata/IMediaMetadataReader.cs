using Lunalipse.Common.Data;

namespace Lunalipse.Common.Interfaces.IMetadata
{
    public interface IMediaMetadataReader
    {
        MusicEntity CreateEntity(string path);
        //T CreateRaw(string path);
    }
}
