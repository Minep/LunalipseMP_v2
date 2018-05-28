using Lunalipse.Common.Data;
using Lunalipse.Common.Interfaces.II18N;

namespace Lunalipse.Common.Interfaces.IMetadata
{
    public interface IMediaMetadataReader
    {
        MusicEntity CreateEntity(string path);
        //T CreateRaw(string path);
    }
}
