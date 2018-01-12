using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Interfaces.II18N
{
    public interface II18NCollection
    {
        string getContext(string indexer);
        bool AddToCollection(string indexer, string context);
        bool RemoveFromCollection(string indexer);
        bool SetCollectionElement(string indexer,string NewContext);
    }
}
