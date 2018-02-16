using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Common.Data.BehaviorScript
{
    public enum DefinedCmd : uint
    {
        CMD_NAN    =  0x0000,
        LUNA_PLAY  =  0x0001,
        LUNA_PLAYN =  0x0002,
        LUNA_PLAYC =  0x0003,
        LUNA_EQZR  =  0x0004,
        LUNA_NEXT  =  0x0005,
        LUNA_LLOOP =  0x0006,
        LUNA_SET   =  0x0007
    }
}
