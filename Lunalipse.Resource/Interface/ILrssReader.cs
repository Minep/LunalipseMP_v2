using Lunalipse.Resource.Generic.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lunalipse.Resource.Interface
{
    public interface ILrssReader
    {
        void LoadLrss(string path);
        bool RestoringMagic(byte[] DecKey = null);
        List<LrssIndex> GetIndex();
        Task<LrssResource> ReadResource(LrssIndex li);
    }
}