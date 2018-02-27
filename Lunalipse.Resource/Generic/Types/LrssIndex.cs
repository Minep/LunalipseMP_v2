using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Resource.Generic.Types
{
    public class LrssIndex
    {
        Structure.LPS_FHEADER __HEADER;

        public long Address { get; private set; }
        public string Name
        {
            get
            {
                return __HEADER.FH_NAME;
            }
        }
        public string Type
        {
            get
            {
                return __HEADER.FH_TYPE;
            }
        }
        public long Size
        {
            get
            {
                return __HEADER.FH_SIZE;
            }
        }
        public int Occupied
        {
            get
            {
                return __HEADER.FH_BCOUNT;
            }
        }

        internal LrssIndex(Structure.LPS_FHEADER HEADER, long Location)
        {
            __HEADER = HEADER;
            Address = Location;
        }
    }
}
