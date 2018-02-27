using Lunalipse.Resource.Generic.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Resource.Interface
{
    public interface ILrssWriter
    {
        Task<bool> Export();
        Task<bool> AppendResource(string path);
        Task<bool> AppendResources(string baseDir);
        Task<bool> AppendResources(params string[] pathes);
        void RemoveResource(int index);
    }
}
