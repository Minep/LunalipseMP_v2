using Lunalipse.Resource.Generic.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lunalipse.Resource.Generic.Structure;

namespace Lunalipse.Resource.Interface
{
    public interface ILrssReader
    {
        LrssIndex[] GetIndex();
        Task<LrssResource> ReadResource(LrssIndex li);
    }
}
