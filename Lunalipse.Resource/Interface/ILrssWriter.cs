using System.Threading.Tasks;

namespace Lunalipse.Resource.Interface
{
    public interface ILrssWriter
    {
        Task<bool> Export();
        bool AppendResource(string path);
        bool AppendResourcesDir(string baseDir);
        bool AppendResources(params string[] pathes);
        void RemoveResource(int index);
    }
}