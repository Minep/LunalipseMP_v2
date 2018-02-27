using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.ICommunicator
{
    public interface IGeneralExporter<T>
    {
        bool Export(T instance, string selector);
    }
}
