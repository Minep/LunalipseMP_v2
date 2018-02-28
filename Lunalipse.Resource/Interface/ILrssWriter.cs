using System.Threading.Tasks;

namespace Lunalipse.Resource.Interface
{
    public interface ILrssWriter
    {
        Task<bool> Export();
        Task<bool> AppendResource(string path);
        Task<bool> AppendResourcesDir(string baseDir);
        Task<bool> AppendResources(params string[] pathes);
        void RemoveResource(int index);
    }
}