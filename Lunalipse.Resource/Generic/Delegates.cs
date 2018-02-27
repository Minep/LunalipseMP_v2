using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Resource.Generic
{
    public class Delegates
    {
        public delegate void ChuckOperated(int operated, int total);
        public delegate void EndpointReached(params object[] args);
    }
}
