using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Resource.Generic
{
    public static class Structure
    {
        public struct LPS_HEADER
        {
            public int H_MAGIC;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string H_SIG;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public long[] H_FH_LOC;
            public bool H_ENCRYPTED;
        };

        internal struct LPS_FHEADER
        {
            public int FH_INDEX;
            public int FH_BCOUNT;
            public long FH_SIZE;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string FH_NAME;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string FH_TYPE;
        };

        internal struct LPS_FBLOCK
        {
            public int FB_INDEX;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public byte[] FB_DAT;
        };
    }
}
