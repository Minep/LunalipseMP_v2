using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.ICommunicator
{
    public interface IGeneralImporter<T>
    {
        T Import();
        bool Import(T instance, string selector);
        string[] GetExtra();
    }
}
